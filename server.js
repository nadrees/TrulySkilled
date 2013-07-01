var port = 8888;
var configured = false;

var express = require('express'),
    app = express(),
    cons = require('consolidate'),
    swig = require('swig'),
    fs = require('fs'),
    passport = require('passport'),
    GoogleStragey = require('passport-google').Strategy,
    mongoose = require('mongoose'),
    controllers = require('./lib/controllers'),
    passportMiddleware = require('./lib/passport-middleware');

var openIdReturnUrl = '';
var realm = '';
var dbConnectionString = '';

app.configure('dev', function() {
    console.log('configuring for development');

    openIdReturnUrl = 'http://localhost:' + port + '/auth/google/return';
    realm = 'http://localhost:' + port;
    dbConnectionString = 'mongodb://localhost/trulyskilled';

    configured = true;
});

if (!configured) {
    console.log('ERROR: env specific configuration not called');
    process.exit(1);
}

app.configure(function() {
    app.use(express.static('public'));
    app.use(express.logger());
    app.use(express.cookieParser());
    app.use(express.bodyParser());
    app.use(express.session({ secret: fs.readFileSync('./lib/.session', 'utf8') }));
    app.use(express.csrf());
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
});

app.get('/', controllers.index);
app.get('/auth/google', passport.authenticate('google'));
app.get('/auth/google/return', passport.authenticate('google', {
    successRedirect: '/',
    failureRedirect: '/',
    failureFlash: true
}));