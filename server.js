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
    controllers = require('./lib/controllers'),
    passportMiddleware = require('./lib/passport-middleware');

var openIdReturnUrl = '';
var realm = '';

if (app.get('env') === 'dev' || app.get('env') === 'test') {
    openIdReturnUrl = 'http://localhost:' + port + '/auth/google/return';
    realm = 'http://localhost:' + port;

    configured = true;
}

if (app.get('env') === 'production') {
    // get traffic logs
    app.use(express.logger());

    configured = true;
}

if (!configured) {
    console.log('ERROR: env specific configuration not called');
    process.exit(1);
}

app.use(express.static('public'));
app.use(express.cookieParser());
app.use(express.bodyParser());
app.use(express.session({ 
    secret: fs.readFileSync('./lib/.session', 'utf8'),
    cookie: { maxAge: 60000 }
}));
app.use(express.csrf());
app.use(flash());
app.use(passport.initialize());
app.use(passport.session());
app.use(app.router);

app.engine('.html', cons.swig);
app.set('view engine', 'html');

swig.init({
    root: __dirname + '/views',
    allowError: true
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
    console.log('listening on port ' + port);
});

app.get('/', controllers.index);
app.get('/auth/google', passport.authenticate('google'));
app.get('/auth/google/return', passport.authenticate('google', {
    successRedirect: '/',
    failureRedirect: '/',
    failureFlash: true
}));
app.get('/logout', controllers.logout);
app.get('/user', controllers.user.index);

module.exports = app;