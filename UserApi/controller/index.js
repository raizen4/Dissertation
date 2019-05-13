const NewRouter = require('restify-router').Router;

const router = new NewRouter();
router.get('/', (req, res, next) => {
  res.json({
    message: 'Welcome to API',
    query: req.query,
  });
  next();
});

router.get('/:name', (req, res, next) => {
  res.json({
    message: `Welcome to API ${req.params.name}`,
    query: req.query,
  });
  next();
});

router.post('/', (req, res, next) => {
  res.json({
    message: 'Welcome to UserApi',
    query: req.query,
  });
  next();
});

module.exports = router;
