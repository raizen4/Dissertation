/* eslint-disable no-undef */
const NewRouter = require('restify-router').Router;
const BaseResponse = require('../serviceModels/BaseResponse');

const router = new NewRouter();

router.post('/SendSms', async (req, res) => {
  try {
    let from = '4407459471696';
    let to = req.body.ContactDetails.Sms;
    // const parsedBody = JSON.parse(req.body);
    from = `+${from}`;

    // this is very important so make sure you have included + sign before ISD code to send sms
    to = `+${to}`;

    session.submit_sm({
      source_addr: from,
      destination_addr: to,
      short_message: text,
    }, (pdu) => {
      if (pdu.command_status === 0) {
        // Message successfully sent
        console.log(pdu.message_id);
        const newResp = new BaseResponse();
        newResp.HasBeenSuccessful = true;
        newResp.Errors = null;
        res.send(newResp);
      } else {
        const newResp = new BaseResponse();
        newResp.HasBeenSuccessful = false;
        newResp.Errors = 'Internal server error';
        res.send(newResp);
      }
    });
  } catch (err) {
    const newResp = new BaseResponse();
    newResp.HasBeenSuccessful = false;
    newResp.Errors = err;
    res.send(newResp);
  }
});

module.exports = router;
