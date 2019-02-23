/* eslint-disable no-underscore-dangle */
/* eslint-disable no-undef */
require('mongoose');
const jwt = require('jsonwebtoken');
// eslint-disable-next-line import/no-extraneous-dependencies
const bcrypt = require('bcrypt');
const constants = require('../Constants').default;
const User = require('../DbSchemas/UserSchema');


async function register(userToRegister) {
  const newUser = new User(userToRegister);
  newUser.HashedPass = bcrypt.hashSync(userToRegister.Password, 10);
  try {
    const responseFromDb = await newUser.save();
    if (responseFromDb != null) {
      return true;
    }
    return false;
  } catch (err) {
    console.log(err);
    return false;
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

async function AddPin(userId, pin) {
  try {
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

async function AddNewActionForLocker(userId, action) {
  try {
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
      RemovePin(action.Pin.Code);
      break;
    case constants.Actions.PickingUp:
      newAction = `Picked up code used on ${currentDate} using pin ${action.Pin.Code}`;
      RemovePin(action.Pin.Code);
      break;
    default:
      break;
    }
    const result = await User.findByIdAndUpdate(
      userId,
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
};
