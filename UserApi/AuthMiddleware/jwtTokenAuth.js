
const jwt = require('jsonwebtoken');
const constants = require('../Constants');
const BaseResponse = require('../serviceModels/BaseResponse');

// eslint-disable-next-line consistent-return
function checkToken(req, res, next) {
  const parsedBody = req.body;
  let token = req.headers.authorization; // Express headers are auto converted to lowercase
  if (token) {
    if (token.startsWith('Bearer ')) {
      // Remove Bearer from string
      token = token.slice(7, token.length);
    }
    jwt.verify(token, constants.secret, (err, decoded) => {
      if (err) {
        const notValidResp = new BaseResponse();
        notValidResp.Errors = 'Token not valid. Unauthorized';
        notValidResp.HasBeenSuccessful = false;
        return res.send(notValidResp);
      }
      req.body = parsedBody;
      req.body.User = decoded;
      return next();
    });
  } else {
    const notSuppliedRes = new BaseResponse();
    notSuppliedRes.Errors = 'Token not provided';
    notSuppliedRes.HasBeenSuccessful = false;
    return res.send(notSuppliedRes);
  }
}

module.exports = {
  checkToken,
};
