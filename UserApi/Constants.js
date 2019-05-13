// eslint-disable-next-line no-unused-vars
const secret = 'dSDer3rfsdgvasvhjopdy73621d';
const Actions = {
  UserAppOpened: 1,
  UserAppClosed: 0,
  Delivered: 2,
  PickedUp: 3,
  CheckConnection: 8,
};

const SmsActions = {
  SendNewPinToUser: 1,
  Delivered: 2,
  PickedUp: 3,
  PowerStatusChangedToBackup: 4,
  PowerStatusChangedToMain: 5,
};


const PowerStatuses = {
  MainPower: 1,
  BackupPower: 2,

};

const ApiEndpoints = {
  Email: {
    EmailGenerateConfirmPinCreation: 'http://192.168.88.30:4004/EmailApi/emails/GenerateConfirmPinCreation',
    GenerateEmailForPicker: 'http://192.168.88.30:4004/EmailApi/emails/GenerateEmailForPicker',
    LockerActionSucceded: 'http://192.168.88.30:4004/EmailApi/emails/LockerActionSucceded',
  },
  GenerateSms: 'http://192.168.88.30:4005/SmsApi/sms/SendSms',
};
const PickerTypes = {
  Friend: 1,
  Courier: 2,
};
module.exports = {
  secret, Actions, PickerTypes, ApiEndpoints, SmsActions, PowerStatuses,
};
