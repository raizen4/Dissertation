// eslint-disable-next-line no-unused-vars
const secret = 'dSDer3rfsdgvasvhjopdy73621d';
const Actions = {
  Opened:1,
  Closed:0,
  Delivered:2,
  PickedUp:3,
  RequestPinApproval:4,
  UserAppClose:5,
  UserAppOpen:6,
  CheckConnection:8,
};

const ApiEndpoints = {
  Email: 'http://localhost:4003/EmailApi/SendEmail',
  Sms: 'http://localhost:4004/SmsApi/SendSmss',
};
const PickerTypes = {
  Friend: 1,
  Courier: 2,
};
export default {
  secret, Actions, PickerTypes, ApiEndpoints,
};
