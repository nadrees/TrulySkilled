var port = process.env.PORT;
var configured = false;
var dbConnectionString = process.env.DB_CONNECTION_STRING;

var express = require('express'),
    app = express(),
    cons = require('consolidate'),
    swig = require('swig'),
    fs = require('fs'),
    passport = require('passport'),
    GoogleStragey = require('passport-google').Strategy,
    flash = require('connect-flash'),
    mongoose = require('mongoose'),
    winston = require('winston'),
    controllers = require('./lib/controllers'),
    passportMiddleware = require('./lib/passport-middleware');

var openIdReturnUrl = '';
var realm = '';

if (app.get('env') === 'dev' || app.get('env') === 'test') {
    openIdReturnUrl = 'http://localhost:' + port + '/auth/google/return';
    realm = 'http://localhost:' + port;

    configured = true;
}

if (app.get('env') === 'production' || app.get('env') === 'dev') {
    var logStream = fs.createWriteStream('./traffic.log');
    app.use(express.logger({stream: logStream}));

    configured = true;
}

if (!configured) {
    console.log('ERROR: env specific configuration not called');
    process.exit(1);
}

winston.add(winston.transports.File, {
    filename: 'logs.log',
    maxsize: 1073741824, // 1 MB
    maxFiles: 10
});
// middleware for injecting objects into req and res
app.use(function(req, res, next) {
    req.logger = winston;
    next();
});

// finish setting up middleware
app.use(express.cookieParser());
app.use(express.bodyParser());
app.use(express.session({ 
    secret: fs.readFileSync('./lib/.session', 'utf8')
}));
app.use(express.csrf());
app.use(flash());
app.use(passport.initialize());
app.use(passport.session());

app.engine('.html', cons.swig);
app.set('view engine', 'html');

swig.init({
    root: __dirname + '/views',
    allowError: true,
    cache: app.get('env') === 'production'
});

app.set('views', __dirname + '/views/pages');
app.set('view options', { layout: false });

/* passport setup */
passport.use(new GoogleStragey({
        returnURL: openIdReturnUrl,
        realm: realm
    },
    passportMiddleware.handleReturn
));
passport.serializeUser(passportMiddleware.serializeUser);
passport.deserializeUser(passportMiddleware.deserializeUser);

/* mongoose setup */
mongoose.connect(dbConnectionString);
var db = mongoose.connection;
db.on('error', console.error.bind(console, 'connection error:'));
db.once('open', function() {
    // start server
    app.listen(port);
    winston.log('info', 'listening on port %d', port);
});

/*******************
/****** Routes *****
*******************/
app.get('/', controllers.index);
app.get('/auth/google', passport.authenticate('google'));
app.get('/auth/google/return', passport.authenticate('google', {
    successRedirect: '/',
    failureRedirect: '/',
    failureFlash: true
}));
app.get('/logout', controllers.logout);
app.get('/user', controllers.user.index);
app.post('/user', controllers.user.update);

app.use(express.static('public'));

module.exports = app;