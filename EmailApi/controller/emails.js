/* eslint-disable no-undef */
const NewRouter = require('restify-router').Router;
const nodemailer = require('nodemailer');
const BaseResponse = require('../serviceModels/BaseResponse');
const actions = require('../serviceModels/LockerActionsEnum');

const router = new NewRouter();


router.post('/LockerActionSucceded', async (req, res) => {
  try {
    const transporter = nodemailer.createTransport({
      service: 'gmail',
      auth: {
        user: 'bboldurdissertation@gmail.com',
        pass: 'Bogdan.95',
      },
    });
    // const parsedBody = JSON.parse(req.body);
    const receiver = req.body.Email;
    const sender = 'bboldurdissertation@gmail.com';
    const pickerName = req.body.PickerName;
    const deliveryCompanyName = req.body.DeliveryCompanyName;
    const actionRequired = req.body.ActionType;
    const pinUsed = req.body.PinCode;
    let textMessage;
    let textTitle;
    const currentDate = new Date().toISOString()
      .replace(/T/, ' ') // replace T with a space
      .replace(/\..+/, '');
    switch (actionRequired) {
    case actions.Delivered:
      textTitle = 'Delivered';
      textMessage = `One of your parcels has been delivered at ${currentDate} by ${deliveryCompanyName} using the following pin: ${pinUsed}`;
      break;
    case actions.PickedUp:
      textTitle = 'Picked Up';
      textMessage = `One of your parcels has been picked up at ${currentDate} by ${pickerName} using the following pin: ${pinUsed}`;
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


router.post('/GenerateConfirmPinCreation', async (req, res) => {
  try {
    const transporter = nodemailer.createTransport({
      service: 'gmail',
      auth: {
        user: 'bboldurdissertation@gmail.com',
        pass: 'Bogdan.95',
      },
    });
    // const parsedBody = JSON.parse(req.body);
    const receiver = req.body.Email;
    const sender = 'bboldurdissertation@gmail.com';
    const pinUsed = req.body.PinCode;
    const textTitle = 'Pin Created';
    const textMessage = `The following pin has been created: ${pinUsed} .
       You can see it in the current pins section of the mobile app`;

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


router.post('/GenerateEmailForPicker', async (req, res) => {
  try {
    const transporter = nodemailer.createTransport({
      service: 'gmail',
      auth: {
        user: 'bboldurdissertation@gmail.com'.trim(),
        pass: 'Bogdan.95'.trim(),
      },
    });
    // const parsedBody = JSON.parse(req.body);
    const receiver = req.body.Email;
    const sender = 'bboldurdissertation@gmail.com';
    const pinUsed = req.body.PinCode;
    const textTitle = 'Pin Created';
    const textMessage = `The following pin has been created for you to be able to access to content of the locker. The pin is the following ${pinUsed}`;

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
