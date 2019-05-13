const requestPromise = require('request-promise');

async function RequestServiceMethod(req, redirectUri, httpVerb) {
  const options = {
    method: httpVerb,
    uri: redirectUri,
    json: true, // Automatically stringifies the body to JSON
  };
  options.body = req;
  const parsedBody = await requestPromise(options);
  try {
    if (parsedBody.HasBeenSuccessful) {
      return true;
    }
    return false;
  } catch (err) {
    return false;
  }
}

module.exports = { RequestServiceMethod };
