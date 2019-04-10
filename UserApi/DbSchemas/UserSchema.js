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

const ContactDetails = new Schema({
  PickerName: {
    type: String,
    required: true,
  },
  DeliveryCompanyName: {
    type: String,
    required: true,
  },
  Email: {
    type: String,
    default: undefined,
    required: false,
  },
  Phone: {
    type: String,
    default: undefined,
    required: false,

  },

});

const Pin = new Schema({
  Code: {
    type: String,
    required: true,
  },
  UserType: {
    type: Number,
    required: true,
  },
  Created: {
    type: String,
    default: undefined,
  },
  ParcelContactDetails: {
    type: ContactDetails,
    default: undefined,
    required: false,
  },
});


const Locker = new Schema({
  DeviceId: {
    type: String,
    required: true,
  },
  IotHubConnectionString: {
    type: String,
    required: false,
    default: undefined,
  },
  History: [LockerActivity],
  ActivePins: [Pin],
});

const UserSchema = new Schema({
  DisplayName: {
    type: String,
    required: false,
    trim: true,
  },
  DeviceId: {
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
  Phone: {
    type: String,
    required: false,
    default: undefined,
  },
  IoTHubConnectionString: {
    type: String,
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
