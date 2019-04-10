const restify = require('restify');
// eslint-disable-next-line global-require
const router = new (require('restify-router')).Router();
// init db and all schemas
require('./DbSchemas/DbInit');
require('./DbSchemas/UserSchema');
const cors = require('cors');
const logger = require('./basic-logger');

const port = 4000;
const server = restify.createServer({
  name: 'UserApi',
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


const homeController = require('./controller/index');
const userController = require('./controller/users');

router.add('/UserApi', homeController);
router.add('/UserApi/users', userController);
router.applyRoutes(server);

server.on('after', restify.plugins.metrics({ server }, (err, metrics) => {
  logger.trace(`${metrics.method} ${metrics.path} ${metrics.statusCode} ${metrics.latency} ms`);
}));

if (process.env.PORT != null) {
  server.listen(process.env.PORT, '192.168.88.30', () => {
    console.log('%s listening at process.end.port %s', server.name, server.url);
    logger.info('%s listening at %s', server.name, server.url);
  });
} else {
  server.listen(port, '192.168.88.30', () => {
    console.log('%s listening at %s', server.name, server.url);
    logger.info('%s listening at %s', server.name, server.url);
  });
}
server.on('uncaughtException', (req, res, route, err) => {
  logger.error(err);
});
