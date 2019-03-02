// eslint-disable-next-line no-unused-vars
const secret = 'dSDer3rfsdgvasvhjopdy73621d';
const Actions = {
  Open: 'Open',
  Close: 'Close',
  Delivery: 'Delivery',
  PickingUp: 'PickingUp',
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
