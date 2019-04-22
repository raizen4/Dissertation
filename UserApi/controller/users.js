/* eslint-disable max-len */
/* eslint-disable no-undef */
const NewRouter = require('restify-router').Router;
const userManager = require('../manager/userManager');
const BaseResponse = require('../serviceModels/BaseResponse');
const ResponseData = require('../serviceModels/ResponseData');
const jwtChecker = require('../AuthMiddleware/jwtTokenAuth');

const router = new NewRouter();
router.post('/login', async (req, res) => {
  const email = req.body.Email;
  const pass = req.body.Password;
  const loginType = req.body.LoginType;
  let managerResult;
  if (loginType !== undefined && loginType === 'LOCKER') {
    managerResult = await userManager.LoginLocker(email, pass);
  } else {
    managerResult = await userManager.login(email, pass);
  }

  if (managerResult != null) {
    const newResp = new ResponseData();
    newResp.Content = managerResult;
    newResp.HasBeenSuccessful = true;
    newResp.Errors = null;
    res.send(newResp);
  } else {
    const newResp = new ResponseData();
    newResp.Content = null;
    newResp.HasBeenSuccessful = false;
    newResp.Errors = 'Password or email wrong. Please try again';
    res.send(newResp);
  }
});

router.post('/register', async (req, res) => {
  console.log(`register hit${req.body}`);
  const User = {
    DisplayName: req.body.DisplayName,
    Email: req.body.Email,
    HashedPassword: req.body.Password,
    Phone: req.body.Phone,
    DeviceId: req.body.DeviceId,

  };
  console.log(User);
  try {
    const managerResult = await userManager.register(User);
    if (managerResult) {
      const newResp = new BaseResponse();
      newResp.HasBeenSuccessful = true;
      newResp.Errors = null;
      res.send(newResp);
    } else {
      const newResp = new BaseResponse();
      newResp.HasBeenSuccessful = false;
      newResp.Errors = 'Email already exists. Please change the email';
      res.send(newResp);
    }
  } catch (err) {
    const newResp = new ResponseData();
    newResp.HasBeenSuccessful = false;
    newResp.Errors = 'Internal server error';
    res.send(newResp);
  }
});


router.post('/RemovePinForLocker', jwtChecker.checkToken, async (req, res) => {
  try {
    // const parsedBody = JSON.parse(req.body);
    const userId = req.body.User.Id;
    const pinToRemove = req.body.Pin.Code;
    const managerResult = await userManager.RemovePin(userId, pinToRemove);
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


router.put('/SendPowerStatusChanged', jwtChecker.checkToken, async (req, res) => {
  try {
    // const parsedBody = JSON.parse(req.body);
    const userId = req.body.User.UserId;
    const currentPowerStatus = req.body.PowerStatus;
    const managerResult = await userManager.SendPowerStatusChanged(userId, currentPowerStatus);
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


router.put('/CheckPin', jwtChecker.checkToken, async (req, res) => {
  try {
    // const parsedBody = JSON.parse(req.body);
    const userId = req.body.User.UserId;
    const pinToCheck = req.body.PinCode;
    const managerResult = await userManager.CheckPin(userId, pinToCheck);
    if (managerResult != null) {
      const newResp = new ResponseData();
      newResp.HasBeenSuccessful = true;
      newResp.Content = managerResult;
      newResp.Errors = null;
      res.send(newResp);
    } else {
      const newResp = new ResponseData();
      newResp.HasBeenSuccessful = false;
      newResp.Content = null;
      newResp.Errors = 'Pin not valid';
      res.send(newResp);
    }
  } catch (err) {
    const newResp = new ResponseData();
    newResp.Content = null;
    newResp.HasBeenSuccessful = false;
    newResp.Errors = err;
    res.send(newResp);
  }
});


router.put('/AddNewActionForLocker', jwtChecker.checkToken, async (req, res) => {
  try {
    // small issue here which will be solved later. The userObject is sent from the phone where I have all
    // the data in the token, the userId is sent by the locker when it tries to add a new action and so I
    // need to retrieve the whole user before accessing the method
    let userObject = req.body.User;
    if (userObject.Id === undefined) {
      const userId = req.body.User.UserId;
      const userFound = await userManager.GetUser(userId);
      if (userFound != null) {
        userObject = userFound;
        userObject.Id = userFound.id;
      } else {
        const newResp = new BaseResponse();
        newResp.HasBeenSuccessful = false;
        newResp.Errors = 'Internal server error';
        res.send(newResp);
      }
    }
    const action = req.body.ActionType;
    const pin = req.body.Pin;
    const managerResult = await userManager.AddNewActionForLocker(userObject, action, pin);
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


router.put('/AddPinForLocker', jwtChecker.checkToken, async (req, res) => {
  try {
    // const parsedBody = JSON.parse(req.body);
    const user = req.body.User;
    const pinToAdd = req.body.Pin;
    const managerResult = await userManager.AddPin(user, pinToAdd);
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

router.post('/CreateLocker', jwtChecker.checkToken, async (req, res) => {
  try {
    // const parsedBody = JSON.parse(req.body);
    const userId = req.body.User.UserId;
    const newLockerId = req.body.LockerId;
    const managerResult = await userManager.RegisterLocker(userId, newLockerId);
    if (managerResult) {
      const newResp = new ResponseData();
      newResp.HasBeenSuccessful = true;
      newResp.Content = managerResult;
      newResp.Errors = null;
      res.send(newResp);
    } else {
      const newResp = new ResponseData();
      newResp.Content = null;
      newResp.HasBeenSuccessful = false;
      newResp.Errors = 'Internal server error';
      res.send(newResp);
    }
  } catch (err) {
    const newResp = new ResponseData();
    newResp.HasBeenSuccessful = false;
    newResp.Content = null;
    newResp.Errors = err;
    res.send(newResp);
  }
});


router.get('/GetActivePins', jwtChecker.checkToken, async (req, res) => {
  try {
    // const parsedBody = JSON.parse(req.body);
    const userId = req.body.User.Id;
    const managerResult = await userManager.GetActivePins(userId);
    if (managerResult) {
      const newResp = new ResponseData();
      newResp.HasBeenSuccessful = true;
      newResp.Content = managerResult;
      newResp.Errors = null;
      res.send(newResp);
    } else {
      const newResp = new ResponseData();
      newResp.HasBeenSuccessful = false;
      newResp.Content = managerResult;
      newResp.Errors = 'Internal server error';
      res.send(newResp);
    }
  } catch (err) {
    const newResp = new ResponseData();
    newResp.HasBeenSuccessful = false;
    newResp.Content = managerResult;
    newResp.Errors = err;
    res.send(newResp);
  }
});
router.get('/GetDeliveryHistory', jwtChecker.checkToken, async (req, res) => {
  try {
    const userId = req.body.User.Id;
    const managerResult = await userManager.GetLockerHistory(userId);
    if (managerResult != null) {
      const newResp = new ResponseData();
      newResp.HasBeenSuccessful = true;
      newResp.Content = managerResult;
      newResp.Errors = null;
      res.send(newResp);
    } else {
      const newResp = new ResponseData();
      newResp.HasBeenSuccessful = false;
      newResp.Content = null;
      newResp.Errors = 'Internal server error';
      res.send(newResp);
    }
  } catch (err) {
    const newResp = new ResponseData();
    newResp.HasBeenSuccessful = false;
    newResp.Content = null;
    newResp.Errors = err;
    res.send(newResp);
  }
});


module.exports = router;
