/* eslint-disable no-undef */
const NewRouter = require('restify-router').Router;
const twilio = require('twilio');
const BaseResponse = require('../serviceModels/BaseResponse');
const Constants = require('../Constants');

const router = new NewRouter();

router.post('/SendSms', async (req, res) => {
  try {
    const accountSid = 'AC15fd244e5b3f038c16e1a370ee94542b'; // Your Account SID from www.twilio.com/console
    const authToken = 'f3157995c9f4e546f2dfff2d835057ab'; // Your Auth Token from www.twilio.com/console


    const pinCode = req.body.PinCode;
    const action = req.body.Action;
    const deliveryCompany = req.body.DeliveryCompany;
    const pickerName = req.body.PickerName;
    const targetedPhone = req.body.Phone.trim();
    const currentDate = new Date().toISOString()
      .replace(/T/, ' ') // replace T with a space
      .replace(/\..+/, '');


    let smsBody = '';
    switch (action) {
    case Constants.SmsActions.SendNewPinToUser:
    {
      smsBody = `Hi, the access pin for the locker is  ${pinCode} .Please do not tell this code to anyone`;
      break;
    }
    case Constants.SmsActions.PickedUp: {
      smsBody = `Hi, your parcel(s) has been picked up by your friend ${pickerName} at ${currentDate} using the following pin ${pinCode}`;
      break;
    }
    case Constants.SmsActions.Delivered: {
      if (deliveryCompany === 'OTHER') {
        smsBody = `Hi, your parcel(s) has been delivered  at ${currentDate} using the following pin ${pinCode}`;
      } else {
        smsBody = `Hi, your parcel(s) has been delivered by ${deliveryCompany} at ${currentDate} using the following pin ${pinCode}`;
      }
      break;
    }
    default:
      break;
    }
    // eslint-disable-next-line new-cap
    const client = new twilio(accountSid, authToken);


    client.messages.create({
      body: smsBody,
      to: targetedPhone, // Text this number
      from: '+447449528194', // From a valid Twilio number
    })
      .then(message => console.log(message.sid))
      .done();
    const newResp = new BaseResponse();
    newResp.HasBeenSuccessful = true;
    newResp.Errors = null;
    res.send(newResp);
  } catch (err) {
    const newResp = new BaseResponse();
    newResp.HasBeenSuccessful = false;
    newResp.Errors = err;
    res.send(newResp);
  }
});

module.exports = router;
