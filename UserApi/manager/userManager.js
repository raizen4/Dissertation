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
const responses = require('../serviceModels/Responses');
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
    const responseFromDb = await newUser.save();
    if (responseFromDb != null) {
      const dataToSendBack = {
        IoTHubConnectionString: hubIdentityCreated.ConnectionString,
        Key: hubIdentityCreated.SAK,
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
    };
    const result = await User.findOneAndUpdate(
      { _id: userId },

      {
        $push: { AccountLocker: newLocker },
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
    LockerId: user.AccountLocker.DeviceId,
    DeviceId: user.DeviceId,
    Email: user.Email,
    Id: user._id,
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
          LockerId: foundUser.AccountLocker.DeviceId,
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

// eslint-disable-next-line complexity
async function AddPin(user, pin) {
  try {
    const result = await User.findOneAndUpdate(
      { _id: user.Id },

      {
        $push: { 'AccountLocker.ActivePins': pin },
      },
    );
    await helpers.GenerateConfirmPinCreation(user.Email);
    if (result != null) {
      const forWho = pin.PickerType;
      if (forWho === constants.PickerTypes.Friend) {
        const phone = pin.ContactDetails.Phone;
        const email = pin.ContactDetails.Email;
        await helpers.GenerateEmail(email);
        await helpers.GenerateSms(phone);
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
    const currentDate = Date().toISOString()
      .replace(/T/, ' ') // replace T with a space
      .replace(/\..+/, '');
    let newAction = '';
    switch (action) {
    case constants.Actions.Open:
      newAction = `Opened at ${currentDate}`;
      break;
    case constants.Actions.Close:
      newAction = `Closed at ${currentDate}`;
      break;
    case constants.Actions.Delivery:
      newAction = `Delivery code used on ${currentDate} by ${pin.ContactDetails.DeliveryCompanyName} courier using pin ${pin.Code}`;
      break;
    case constants.Actions.PickingUp:
      newAction = `Picked up code used on ${currentDate} using pin ${pin.Code} by ${pin.ContactDetails.PickerName}`;
      break;
    default:
      break;
    }
    const result = await User.findByIdAndUpdate(
      user.Id,
      {
        $push: { 'AccountLocker.History': newAction },
      },
    );
    if (result != null && (action === constants.Actions.PickedUp || action === constants.Actions.Delivered)) {
      helpers.LockerActionSucceded(user.Email, action, pin);
      RemovePin(action.Pin.Code);
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
      const history = result.History;
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
      const history = result.ActivePins;
      return history;
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
};
