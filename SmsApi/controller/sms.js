/* eslint-disable no-undef */
const NewRouter = require('restify-router').Router;
const twilio = require('twilio');
const BaseResponse = require('../serviceModels/BaseResponse');

const router = new NewRouter();

router.post('/SendSms', async (req, res) => {
  try {
    const accountSid = 'ACb446768e97511108f6b6f88f01def636'; // Your Account SID from www.twilio.com/console
    const authToken = 'cb1c4bdeefa0f8a7aa2ddefce78ef5ef'; // Your Auth Token from www.twilio.com/console

    // eslint-disable-next-line new-cap
    const client = new twilio(accountSid, authToken);

    const SmsBody = `Hi, the access pin for the locker is  ${req.body.PinCode} .Please do not tell this code to anyone`;

    client.messages.create({
      body: SmsBody,
      to: req.body.Phone, // Text this number
      from: '+447449528194', // From a valid Twilio number
    })
      .then(message => console.log(message.sid));
  } catch (err) {
    const newResp = new BaseResponse();
    newResp.HasBeenSuccessful = false;
    newResp.Errors = err;
    res.send(newResp);
  }
});

module.exports = router;
