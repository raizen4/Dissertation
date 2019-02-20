/* eslint-disable no-underscore-dangle */
/* eslint-disable no-undef */
require('mongoose');
const jwt = require('jsonwebtoken');
// eslint-disable-next-line import/no-extraneous-dependencies
const bcrypt = require('bcrypt');
const constants = require('../Constants');
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
        const formattedSavedArticles = [];
        for (let i = 0; i < foundUser.SavedArticles.length; i += 1) {
          formattedSavedArticles.push(foundUser.SavedArticles[i].Id);
        }
        const userToSendBack = {
          Email: user.Email,
          ProfileName: user.ProfileName,
          Locker: user.AccountLocker,
          Id: user._id,
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

async function DeleteLockerFromProfile(userId) {
  try {
    const result = await User.findOneAndUpdate(
      { _id: userId },

      { $set: { AccountLocker: undefined } },


    );
    if (result != null) {
      return result;
    }
  } catch (err) {
    return null;
  }
}

async function AddLockerToProfile(userId, locker) {
  try {
    const result = await User.findByIdAndUpdate(
      userId,
      {
        $set: {
          AccountLocker: locker,
        },
      },
    );
    if (result != null) {
      return result;
    }
  } catch (err) {
    return null;
  }
}

async function AddNewActionForLocker(userId, action) {
  try {
    const result = await User.findByIdAndUpdate(
      userId,
      {
        $push: { 'AccountLocker.$.History': action },
      },
    );
    if (result != null) {
      return result;
    }
  } catch (err) {
    return null;
  }
}

async function GetLockerHistory(userId) {
  try {
    const result = await User.findOne({ _id: userId });

    if (result != null) {
      const history = result.History;
      return history;
    }
  } catch (err) {
    return null;
  }
}

module.exports = {
  register,
  login,
  AddNewActionForLocker,
  DeleteLockerFromProfile,
  AddLockerToProfile,
  GetLockerHistory,
};
