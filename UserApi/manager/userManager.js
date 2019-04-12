/* eslint-disable max-len */
/* eslint-disable no-case-declarations */
/* eslint-disable complexity */
/* eslint-disable no-underscore-dangle */
/* eslint-disable no-undef */
require('mongoose');
const jwt = require('jsonwebtoken');
// eslint-disable-next-line import/no-extraneous-dependencies
const bcrypt = require('bcrypt');
const constants = require('../Constants');
const User = require('../DbSchemas/UserSchema');
const IotHub = require('../IotHub/hub');
const helpers = require('../Helpers/SmsAndEmailGenerator');

async function register(userToRegister) {
  const newUser = new User(userToRegister);
  newUser.HashedPass = bcrypt.hashSync(userToRegister.HashedPassword, 10);
  try {
    const hubIdentityCreated = await IotHub.CreateDevice(userToRegister.DeviceId);
    if (hubIdentityCreated == null) {
      return null;
    }
    newUser.IoTHubConnectionString = hubIdentityCreated.ConnectionString;
    newUser.DeviceId = hubIdentityCreated.DeviceId;
    const responseFromDb = await newUser.save();
    if (responseFromDb != null) {
      const dataToSendBack = {
        IoTHubConnectionString: hubIdentityCreated.ConnectionString,
        DeviceName: hubIdentityCreated.DeviceId,

      };
      return dataToSendBack;
    }

    return null;
  } catch (err) {
    console.log(err);
    return null;
  }
}

async function RegisterLocker(userId, lockerId) {
  try {
    const hubIdentityCreated = await IotHub.CreateDevice(lockerId);
    if (hubIdentityCreated == null) {
      return null;
    }

    const newLocker = {
      DeviceId: hubIdentityCreated.DeviceId,
      IotHubConnectionString: hubIdentityCreated.ConnectionString,
    };
    const result = await User.findOneAndUpdate(
      { _id: userId },

      {
        $set: { AccountLocker: newLocker },
      },
    );
    if (result != null) {
      return hubIdentityCreated;
    }
    return null;
  } catch (err) {
    console.log(err);
    return null;
  }
}


async function signToken(user) {
  const jwtToken = await jwt.sign({
    DisplayName: user.DisplayName,
    LockerId: user.AccountLocker === undefined ? null : user.AccountLocker.DeviceId,
    DeviceId: user.DeviceId,
    Email: user.Email,
    Phone: user.Phone,
    Id: user._id,
  },
  constants.secret,
  {
    expiresIn: '23h',

  });
  return jwtToken;
}

async function signTokenLocker(locker) {
  const jwtToken = await jwt.sign({
    DeviceId: locker.DeviceId,
    IotHubConnectionString: locker.IotHubConnectionString,
    Id: locker._id,
  },
  constants.secret,
  {
    expiresIn: '23h',

  });
  return jwtToken;
}
async function login(email, password) {
  try {
    const foundUser = await User.findOne({ Email: email });
    if (foundUser != null) {
      const matchPass = foundUser.comparePassword(password);
      if (matchPass) {
        foundUser.HashedPass = null;
        const token = await signToken(foundUser);
        const userToSendBack = {
          DisplayName: foundUser.DisplayName,
          LockerId: foundUser.AccountLocker === undefined ? null : foundUser.AccountLocker.DeviceId,
          DeviceId: foundUser.DeviceId,

          IotHubConnectionString: foundUser.IoTHubConnectionString,
          Token: token,
        };
        return userToSendBack;
      }
    }
  } catch (err) {
    return null;
  }
  return null;
}

async function LoginLocker(email, password) {
  try {
    const foundUser = await User.findOne({ Email: email });
    if (foundUser != null) {
      const matchPass = foundUser.comparePassword(password);
      if (matchPass) {
        foundUser.HashedPass = null;
        const token = await signTokenLocker(foundUser);
        const userToSendBack = {
          DeviceId: locker.DeviceId,
          IotHubConnectionString: locker.IotHubConnectionString,
          Id: locker._id,
          Token: token,
        };
        return userToSendBack;
      }
    }
  } catch (err) {
    return null;
  }
  return null;
}
async function SendPowerCutNotification(email, lockerId) {
  try {
    await helpers.SendPowerCutNotificationToOwner(email, lockerId);
  } catch (exception) {
    console.log(exception);
  }
}

// eslint-disable-next-line complexity
async function AddPin(user, pin) {
  try {
    const currentDate = new Date().toISOString()
      .replace(/T/, ' ') // replace T with a space
      .replace(/\..+/, '');

    const normalizedPin = {
      Code: pin.Code,
      Created: currentDate,
      UserType: pin.UserType,
      ParcelContactDetails: pin.ParcelContactDetails,
    };
    const result = await User.findByIdAndUpdate(user.Id,
      {
        $push: { 'AccountLocker.ActivePins': normalizedPin },
      });
    if (result != null) {
      await helpers.GenerateConfirmPinCreation(user.Email, normalizedPin.Code);
      const forWho = normalizedPin.UserType;
      if (forWho === constants.PickerTypes.Friend) {
        const phone = normalizedPin.ParcelContactDetails.Phone;
        const email = normalizedPin.ParcelContactDetails.Email;
        if (email !== undefined) { await helpers.GenerateEmailForPicker(email, normalizedPin.Code); }
        if (phone !== undefined) { await helpers.GenerateSms(phone, constants.SmsActions.SendNewPinToUser, undefined, pin.ParcelContactDetails.PickerName, normalizedPin.Code); }
      }
      return true;
    }
    return false;
  } catch (err) {
    return false;
  }
}

async function RemovePin(userId, pin) {
  try {
    const result = await User.findByIdAndUpdate(
      userId,
      {
        $pull: { 'AccountLocker.ActivePins': { Code: pin } },
      },

    );
    if (result != null) {
      return true;
    }
    return false;
  } catch (err) {
    return false;
  }
}

async function CheckPin(userId, pinCode) {
  try {
    const result = await User.findOne({ _id: userId });

    if (result != null) {
      const history = result.ActivePins;
      let found = null;
      history.forEach((Pin) => {
        if (Pin.Code === pinCode) {
          found = Pin;
        }
      });
      if (found != null) {
        return found;
      }
    }
    return null;
  } catch (err) {
    return null;
  }
}
// action comes from locker. It has the following structure:
/*
{
  user-from json token
  action:Close/Open/Delivery/PickingUp
  Pin:{
    Code:
    ContactDetails:{
      Email:...
      SMS:...
      PickerName:...
      DeliveryCompanyName:...
    }
  }
}
*/
async function AddNewActionForLocker(user, action, pin) {
  try {
    const currentDate = new Date().toISOString()
      .replace(/T/, ' ') // replace T with a space
      .replace(/\..+/, '');
    let newAction = '';

    switch (action) {
    case constants.Actions.Opened:
      newAction = `Opened at ${currentDate}`;
      break;
    case constants.Actions.Closed:
      newAction = `Closed at ${currentDate}`;
      break;
    case constants.Actions.Delivered:
      newAction = `Delivery on ${currentDate} by ${pin.ParcelContactDetails.DeliveryCompanyName} courier using pin ${pin.Code}`;
      break;
    case constants.Actions.PickedUp:
      newAction = `Picked up code used on ${currentDate} using pin ${pin.Code} by ${pin.ParcelContactDetails.PickerName}`;
      break;
    default:
      break;
    }
    const LockerAction = {
      ActionType: action,
      Message: newAction,
    };
    const result = await User.findByIdAndUpdate(
      user.Id,
      {
        $push: { 'AccountLocker.History': LockerAction },
      },
    );
    if (result != null) {
      await helpers.LockerActionSucceded(user.Email, action, pin.Code, pin.ParcelContactDetails.PickerName, pin.ParcelContactDetails.DeliveryCompanyName);
      if (action === constants.Actions.PickedUp) {
        await helpers.GenerateSms(user.Phone, constants.SmsActions.PickedUp, undefined, pin.ParcelContactDetails.PickerName, pin.Code);
      } else {
        await helpers.GenerateSms(user.Phone, constants.SmsActions.Delivered, pin.ParcelContactDetails.DeliveryCompanyName, undefined, pin.Code);
      }
      await RemovePin(user.Id, pin.Code);
      return true;
    }
    return false;
  } catch (err) {
    return false;
  }
}

async function GetLockerHistory(userId) {
  try {
    const result = await User.findOne({ _id: userId });

    if (result != null) {
      const history = result.AccountLocker.History;
      return history;
    }
    return null;
  } catch (err) {
    return null;
  }
}

async function GetActivePins(userId) {
  try {
    const result = await User.findOne({ _id: userId });

    if (result != null) {
      const history = result.AccountLocker.ActivePins;
      return history.map(x => x.toObject());
    }
    return null;
  } catch (err) {
    return null;
  }
}

module.exports = {
  register,
  login,
  GetActivePins,
  AddPin,
  RemovePin,
  AddNewActionForLocker,
  GetLockerHistory,
  CheckPin,
  RegisterLocker,
  LoginLocker,
  SendPowerCutNotification,
};
