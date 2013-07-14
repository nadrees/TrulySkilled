var self = this;

var done = function(user, done) {
    return done(null, user);
};

var passportStub = function(req, res, next) {
    if (!self.active) {
        return next();
    }

    var passport = {
        deserializeUser: done,
        serializeUser: done,
        _userProperty: 'user',
        _key: 'passport'
    };
    req.__defineGetter__('_passport', function() {
        return {
            instance: passport,
            session: {
                user: self.user
            }
        };
    });
    req.__defineGetter__('user', function() {
        return self.user;
    });
    return next();
};

module.exports = {
    install: function(app) {
        if (app === null) return;
        self.app = app;
        return app.stack.unshift({
            route: '',
            handle: passportStub,
            _id: 'passport.stub'
        });
    },
    uninstall: function(app) {
        if (app === null) return;
        app.stack.forEach(function(middleware, index, stack) {
            if (middleware._id === 'passport.stub') {
                return stack.splice(index, 1);
            }
        });
        self.app = null;
    },
    login: function(user) {
        if (self.app === null) {
            throw new Error('Passport stub is not installed.');
        }
        self.active = true;
        self.user = user;
    },
    logout: function() {
        self.active = false;
    }
};