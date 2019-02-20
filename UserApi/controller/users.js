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
  const managerResult = await userManager.login(email, pass);
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
    Email: req.body.Email,
    Password: req.body.Password,
    Username: req.body.Username,
    DisplayName: req.body.DiplayName,
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
    const newResp = new BaseResponse();
    newResp.HasBeenSuccessful = false;
    newResp.Errors = 'Email already exists. Please change the email';
    res.send(newResp);
  }
});

router.post('/AddLockerToProfile', jwtChecker.checkToken, async (req, res) => {
  try {
    // const parsedBody = JSON.parse(req.body);
    const userId = req.body.User.Id;
    const newLocker = req.body.Locker;
    const managerResult = await userManager.AddLockerToProfile(userId, newLocker);
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

router.post('/DeleteLockerFromProfile', jwtChecker.checkToken, async (req, res) => {
  try {
    // const parsedBody = JSON.parse(req.body);
    const userId = req.body.User.Id;
    const managerResult = await userManager.DeleteLockerFromProfile(userId);
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
router.put('/AddNewActionForLocker', jwtChecker.checkToken, async (req, res) => {
  try {
    // const parsedBody = JSON.parse(req.body);
    const userId = req.body.User.Id;
    const action = req.body.Action;
    const managerResult = await userManager.AddNewActionForLocker(userId, action);
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
