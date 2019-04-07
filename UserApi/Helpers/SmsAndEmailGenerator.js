
const responses = require('../serviceModels/Responses');
const constants = require('../Constants');

async function GenerateEmailForPicker(email) {
  if (email != null) {
    const uri = 'http://localhost:4001/EmailApi/emails/GenerateEmailForPicker';
    const httpVerb = 'POST';
    const body = {
      Email: email,
    };
    try {
      await responses.RequestServiceMethod(body, uri, httpVerb);
    } catch (returnErrResponse) {
      console.log(returnErrResponse);
    }
  }
}
async function GenerateSms(phone) {
  if (phone != null) {
    const uri = constants.ApiEndpoints.Sms;
    const httpVerb = 'POST';
    const body = {
      Phone: phone,
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
    const uri = 'http://localhost:4001/EmailApi/emails/GenerateConfirmPinCreation';
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
  const emailUri = 'http://localhost:4001/EmailApi/emails/LockerActionSucceded';
  const httpVerb = 'POST';
  const emailServiceBody = {
    OwnerEmail: ownerEmail,
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
