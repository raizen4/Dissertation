
const responses = require('../serviceModels/Responses');
const constants = require('../Constants');

async function GenerateEmail(email) {
  if (email != null) {
    const uri = constants.ApiEndpoints.Email;
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
async function GenerateConfirmPinCreation(ownerEmail) {
  await this.GenerateEmail(ownerEmail);
}

async function LockerActionSucceded(ownerEmail, action) {
  const emailUri = constants.ApiEndpoints.Email;
  const httpVerb = 'POST';
  const emailServiceBody = {
    Receiver: ownerEmail,
    Action: action,
  };
  try {
    await responses.RequestServiceMethod(emailServiceBody, emailUri, httpVerb);
  } catch (returnErrResponse) {
    console.log(returnErrResponse);
  }
}

module.exports = {
  GenerateEmail,
  GenerateSms,
  GenerateConfirmPinCreation,
  LockerActionSucceded,
};
