const mongoose = require('mongoose');
const configuration = require('../Configuration');

mongoose.Promise = global.Promise;
mongoose.connect(configuration.mongoConnection, { useNewUrlParser: true });

module.exports = { mongoose };
