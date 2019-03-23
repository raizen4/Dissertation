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

async function register(userToRegister) {
  const newUser = new User(userToRegister);
  newUser.HashedPass = bcrypt.hashSync(userToRegister.Password, 10);
  try {
    if (responseFromDb != null) {
      const hubIdentityCreated = await IotHub.CreateDevice(newUser.AccountDevice);
      if (hubIdentityCreated == null) {
        return false;
      }
      const responseFromDb = await newUser.save();
      if (responseFromDb != null) { return true; }
    }
    return false;
  } catch (err) {
    console.log(err);
    return false;
  }
}

async function RegisterLocker(userId,lockerId) {
  try {
      const hubIdentityCreated = await IotHub.CreateDevice(lockerId);
      if (hubIdentityCreated == null) {
        return null;
      }
      else{
        const newLocker={
          DeviceId:hubIdentityCreated.DeviceId
        }
        const result = await User.findOneAndUpdate(
          { _id: userId },
    
          {
            $push: { 'AccountLocker': newLocker },
          },
        );
        if (result != null) {
          return hubIdentityCreated;
        }
        return null;
      }
  } catch (err) {
    console.log(err);
    return null;
  }
}



async function signToken(user) {
  const jwtToken = await jwt.sign({
    Email: user.Email,
    ProfileName: user.ProfileName,
    Locker: user.AccountLocker,
    Id: user._id,
  },
  constants.secret,
  {
    expiresIn: '10h',

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
          Email: foundUser.Email,
          ProfileName: foundUser.ProfileName,
          AccountLocker: foundUser.AccountLocker,
          Id: foundUser._id,
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
async function AddPin(userId, pin) {
  try {
    const forWho = pin.PickerType;
    if (forWho === constants.PickerTypes.Friend) {
      const phone = pin.ContactDetails.Phone;
      const email = pin.ContactDetails.Email;   
      if (phone != null) {
        const uri = constants.ApiEndpoints.Sms;
        const httpVerb = 'POST';
        const body={
          Phone:phone
        }
        try {
          await responses.RequestServiceMethod(body, uri, httpVerb);
        } catch (returnErrResponse) {
          console.log(returnErrResponse);
        }
      }
      if (email != null) {
        const uri = constants.ApiEndpoints.Email;
        const httpVerb = 'POST';
        const body={
          Email:email
        }
        try {
          await responses.RequestServiceMethod(body, uri, httpVerb);
        } catch (returnErrResponse) {
          console.log(returnErrResponse);
        }
      }
    }
    const result = await User.findOneAndUpdate(
      { _id: userId },

      {
        $push: { 'AccountLocker.ActivePins': pin },
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

async function CheckPin(userId,pinCode){
  try {
    const result = await User.findOne({ _id: userId });

    if (result != null) {
      let history = result.ActivePins;
      let found=null;
      history.forEach(function(Pin){
       if(Pin.Code==pinCode){
         found=Pin;
         break;
       }      
      });
      if(found!=null){
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
  Type:UserClose/UserOpen/Delivery
  DeliveryCompany:null/string
  Pin:{
    Code:
    TTL:
    ContactDetails:{
      Email:...
      SMS:...
    }
  }
}
*/
async function AddNewActionForLocker(user, action) {
  try {
    const emailUri = constants.ApiEndpoints.Email;
    const httpVerb = 'POST';
    const emailServiceBody = {
      Receiver: user.Email,
      Action: action.Type,
    };
    const currentDate = Date().toISOString()
      .replace(/T/, ' ') // replace T with a space
      .replace(/\..+/, '');
    let newAction = '';
    switch (action.Code) {
    case constants.Actions.Open:
      newAction = `Opened at ${currentDate}`;
      break;
    case constants.Actions.Close:
      newAction = `Closed at ${currentDate}`;
      break;
    case constants.Actions.Delivery:
      newAction = `Delivery code used on ${currentDate} by ${action.DeliveryCompany} courier using pin ${action.Pin.Code}`;
      try {
        await responses.RequestServiceMethod(emailServiceBody, emailUri, httpVerb);
      } catch (returnErrResponse) {
        console.log(returnErrResponse);
      }
      RemovePin(action.Pin.Code);
      break;
    case constants.Actions.PickingUp:
      newAction = `Picked up code used on ${currentDate} using pin ${action.Pin.Code}`;
      try {
        await responses.RequestServiceMethod(emailServiceBody, emailUri, httpVerb);
      } catch (returnErrResponse) {
        console.log(returnErrResponse);
      }
      RemovePin(action.Pin.Code);
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
    if (result != null) {
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
  RegisterLocker
};
