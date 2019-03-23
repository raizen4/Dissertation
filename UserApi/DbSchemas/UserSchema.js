'use-strict';

const mongoose = require('mongoose');
// eslint-disable-next-line import/no-extraneous-dependencies
const bcrypt = require('bcrypt');
// eslint-disable-next-line prefer-destructuring
const Schema = mongoose.Schema;
const LockerActivity = new Schema({
  Action: {
    type: String,
    required: true,
  },
  Created: {
    type: Date,
    default: Date.now,
  },

});
const Pin = new Schema({
  Code: {
    type: String,
    required: true,
  },
  PickerType: {
    type: Number,
    required: true,
  },
  Ttl: {
    type: String,
    required: true,
  },
  Created: {
    type: Date,
    default: Date.now,
  },

});
const Locker = new Schema({
  DeviceId: {
    type: String,
    required: true,
  },
  History: [LockerActivity],
  ActivePins: [Pin],
});

const UserSchema = new Schema({
  ProfileName: {
    type: String,
    required: false,
    trim: true,
  },
  HashedPass: {
    type: String,
    required: true,
    trim: true,
  },
  Email: {
    type: String,
    required: true,
    trim: true,
    unique: true,
  },
  AccountLocker: {
    type: Locker,
    required: false,
    default: undefined,
  },
  Created: {
    type: Date,
    default: Date.now,
  },
});

UserSchema.methods.comparePassword = function (password) {
  return bcrypt.compareSync(password, this.HashedPass);
};
module.exports = mongoose.model('User', UserSchema);
