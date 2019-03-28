/* eslint-disable no-undef */
const NewRouter = require('restify-router').Router;
const nodemailer = require('nodemailer');
const BaseResponse = require('../serviceModels/BaseResponse');
const actions = require('../serviceModels/LockerActionsEnum');
const messages = require('../serviceModels/PredefinedMessages');

const router = new NewRouter();


router.post('/SendEmail', async (req, res) => {
  try {
    var transporter = nodemailer.createTransport({
      service: 'gmail',
      auth: {
        user: 'bboldurdissertation@gmail.com',
        pass: 'Bogdan.95'
      }
    });
    // const parsedBody = JSON.parse(req.body);
    const receiver = req.Email;
    const sender = 'boldurbogdan@yahoo.com';
    const actionRequired = req.Action;
    const pinUsed = req.Pin.Code;
    let textMessage;
    let textTitle;
    switch (actionRequired) {
    case actions.MessageDelivered:
      textTitle = 'Delivered';
      textMessage = messages.MessageDelivered + pinUsed;
      break;
    case actions.PickedUp:
      textTitle = 'Picked Up';
      textMessage = messages.PickedUp + pinUsed;
      break;
    default:
    }

    // setup email data with unicode symbols
    const mailOptions = {
      from: sender, // sender address
      to: receiver, // list of receivers
      subject: textTitle, // Subject line
      text: textMessage, // plain text body
    };

    // send mail with defined transport object
    const managerResult = await transporter.sendMail(mailOptions);
    console.log('Message sent: %s', info.messageId);
    // Preview only available when sending through an Ethereal account
    console.log('Preview URL: %s', nodemailer.getTestMessageUrl(info));

    // Message sent: <b658f8ca-6296-ccf4-8306-87d57a0b4321@example.com>
    // Preview URL: https://ethereal.email/message/WaQKMgKddxQDoou...
    if (managerResult) {
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
  } catch (err) {
    const newResp = new BaseResponse();
    newResp.HasBeenSuccessful = false;
    newResp.Errors = err;
    res.send(newResp);
  }
});

module.exports = router;
