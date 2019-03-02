const restify = require('restify');
// eslint-disable-next-line global-require
const router = new (require('restify-router')).Router();
// init db and all schemas
const cors = require('cors');
const smpp = require('smpp');
const logger = require('./basic-logger');

const port = 4005;
const server = restify.createServer({
  name: 'SmsApi',
  version: '1.0.0',
});

server.use(restify.plugins.throttle({
  burst: 100, // Max 10 concurrent requests (if tokens)
  rate: 2, // Steady state: 2 request / 1 seconds
  ip: true,	// throttle per IP
}));
server.use(restify.plugins.bodyParser({ urlencoded: 'extended' }));
server.use(restify.plugins.acceptParser(server.acceptable));
server.use(restify.plugins.queryParser());
server.use(restify.plugins.gzipResponse());
server.use(cors());


const session = new smpp.Session({ host: '0.0.0.0', port: 9500 });
let isConnected = false;
session.on('connect', () => {
  isConnected = true;

  session.bind_transceiver({
    system_id: 'USER_NAME',
    password: 'USER_PASSWORD',
    interface_version: 1,
    system_type: '380666000600',
    address_range: '+380666000600',
    addr_ton: 1,
    addr_npi: 1,
  }, (pdu) => {
    if (pdu.command_status === 0) {
      console.log('Successfully bound');
    }
  });
});

session.on('close', () => {
  console.log('smpp is now disconnected');

  if (isConnected) {
    session.connect(); // reconnect again
  }
});

session.on('error', (error) => {
  console.log('smpp error', error);
  isConnected = false;
});

const smsController = require('./controller/sms');

router.add('/UserApi/users', smsController);
router.applyRoutes(server);

server.on('after', restify.plugins.metrics({ server }, (err, metrics) => {
  logger.trace(`${metrics.method} ${metrics.path} ${metrics.statusCode} ${metrics.latency} ms`);
}));

if (process.env.PORT != null) {
  server.listen(process.env.PORT, () => {
    console.log('%s listening at process.end.port %s', server.name, server.url);
    logger.info('%s listening at %s', server.name, server.url);
  });
} else {
  server.listen(port, () => {
    console.log('%s listening at %s', server.name, server.url);
    logger.info('%s listening at %s', server.name, server.url);
  });
}
server.on('uncaughtException', (req, res, route, err) => {
  logger.error(err);
});
