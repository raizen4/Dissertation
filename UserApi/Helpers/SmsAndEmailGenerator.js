
const responses = require('../serviceModels/Responses');
const constants = require('../Constants');

async function GenerateEmailForPicker(email, pinCode) {
  if (email != null) {
    const uri = constants.ApiEndpoints.Email.GenerateEmailForPicker;
    const httpVerb = 'POST';
    const body = {
      Email: email,
      PinCode: pinCode,
    };
    try {
      await responses.RequestServiceMethod(body, uri, httpVerb);
    } catch (returnErrResponse) {
      console.log(returnErrResponse);
    }
  }
}
async function GenerateSms(phone, action, deliveryCompany, pickerName, pin) {
  if (phone != null) {
    const uri = constants.ApiEndpoints.GenerateSms;
    const httpVerb = 'POST';
    const body = {
      Phone: phone,
      Action: action,
      DeliveryCompany: deliveryCompany,
      PickerName: pickerName,
      PinCode: pin,
    };
    try {
      await responses.RequestServiceMethod(body, uri, httpVerb);
    } catch (returnErrResponse) {
      console.log(returnErrResponse);
    }
  }
}
async function GenerateConfirmPinCreation(ownerEmail, pinCode) {
  if (ownerEmail != null) {
    const uri = constants.ApiEndpoints.Email.EmailGenerateConfirmPinCreation;
    const httpVerb = 'POST';
    const body = {
      Email: ownerEmail,
      PinCode: pinCode,
    };
    try {
      await responses.RequestServiceMethod(body, uri, httpVerb);
    } catch (returnErrResponse) {
      console.log(returnErrResponse);
    }
  }
}

async function SendPowerCutNotificationToOwner(ownerEmail, lockerId) {
  if (ownerEmail != null) {
    const uri = 'http://localhost:4001/EmailApi/emails/SendPowerCutNotificationToOwner';
    const httpVerb = 'POST';
    const body = {
      Email: ownerEmail,
      LockerId: lockerId,
    };
    try {
      await responses.RequestServiceMethod(body, uri, httpVerb);
    } catch (returnErrResponse) {
      console.log(returnErrResponse);
    }
  }
}

async function LockerActionSucceded(ownerEmail, action, pin) {
  const emailUri = constants.ApiEndpoints.Email.LockerActionSucceded;
  const httpVerb = 'POST';
  const emailServiceBody = {
    Email: ownerEmail,
    Action: action,
    PinCode: pin,
  };
  try {
    await responses.RequestServiceMethod(emailServiceBody, emailUri, httpVerb);
  } catch (returnErrResponse) {
    console.log(returnErrResponse);
  }
}

module.exports = {
  GenerateEmailForPicker,
  GenerateSms,
  GenerateConfirmPinCreation,
  LockerActionSucceded,
  SendPowerCutNotificationToOwner,
};
