var port = 8888;
var configured = false;

var express = require('express'),
    app = express(),
    cons = require('consolidate'),
    swig = require('swig'),
    flash = require('connect-flash'),
    passport = require('passport'),
    GoogleStragey = require('passport-google').Strategy,
    controllers = require('./lib/controllers'),
    passportMiddleware = require('./lib/passport-middleware');

var openIdReturnUrl = '';
var realm = '';

app.configure('dev', function() {
    console.log('configuring for development');

    openIdReturnUrl = 'http://localhost:' + port + '/auth/google/return';
    realm = 'http://localhost:' + port;

    configured = true;
});

app.configure(function() {
    app.use(express.static('public'));
    app.use(express.logger());
    app.use(express.cookieParser());
    app.use(express.bodyParser());
    app.use(express.session({ secret: 'NRz9H2T#deVaNCBw^QWh4PjyM@4atjHzc1EtSAOjphBilM#xjs*6e*6dkoRYfptMtE6W6jNBw6D$uLadGQg!eZU7%X7qApz0tx2a' }));
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

    passport.use(new GoogleStragey({
            returnURL: openIdReturnUrl,
            realm: realm
        },
        passportMiddleware.handleReturn
    ));
    passport.serializeUser(passportMiddleware.serializeUser);
    passport.deserializeUser(passportMiddleware.deserializeUser);
});

if (!configured) {
    console.log('ERROR: env specific configuration not called');
    process.exit(1);
}

app.get('/', controllers.index);
app.get('/auth/google', passport.authenticate('google'));
app.get('/auth/google/return', passport.authenticate('google', {
    successRedirect: '/',
    failureRedirect: '/',
    failureFlash: true
}));

app.listen(port);
console.log('listening on port ' + port);