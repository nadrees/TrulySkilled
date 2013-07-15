var path = require('path'),
    controllers = require(path.join(process.cwd(), 'lib', 'controllers.js')),
    User = require(path.join(process.cwd(), 'lib', 'models.js')).User,
    should = require('should');

function logInUser(req, user) {
    req.user = user;
}

var request = function() {
    var self = this;

    self.flashes = {};
    self.session = {
        _csrf: ''
    };

    return self;
};
request.prototype.flash = function(type, message) {
    if (message === undefined) {
        return this.flashes[type];
    }
    else {
        this.flashes[type] = message;
    }
};
request.prototype.isAuthenticated = function() {
    return this.user !== undefined && this.user !== null;
};
request.prototype.logout = function() {
    this.user = null;
};

var response = function() {
    var self = this;

    var redirectLocation;

    return self;
};
response.prototype.redirect = function(path) {
    this.redirectLocation = path;
};
response.prototype.render = function(name, args) {
    this.view = {
        name: name,
        args: args
    };
};

var shared = {
    shouldAllowAccess: function() {
        it('should allow access', function() {
            should.exist(this.res.view);
            should.exist(this.res.view.name);
            should.exist(this.res.view.args);
        });
    },
    shouldRender: function(name) {
        it('should render ' + name, function() {
            this.res.view.name.should.equal(name);
        });
    },
    shouldRedirect: function(name) {
        it('should redirect to ' + name, function() {
            should.not.exist(this.res.view);
            should.exist(this.res.redirectLocation);
            this.res.redirectLocation.should.equal(name);
        });
    }
};

describe('controllers', function() {
    beforeEach(function() {
        this.req = new request();
        this.res = new response();
    });

    describe('index', function() {
        describe('with no logged in user', function() {
            beforeEach(function() {
                controllers.index(this.req, this.res);
            });

            shared.shouldAllowAccess();
            shared.shouldRender('index');
        });

        describe('with a logged in user', function() {
            var user;

            beforeEach(function() {
                user = new User({ googleToken: 'test', displayName: 'test' });
                logInUser(this.req, user);
                controllers.index(this.req, this.res);
            });

            shared.shouldAllowAccess();
            shared.shouldRender('index');

            it('should set the user property', function() {
                should.exist(this.res.view.args.user);
            });

            it('should not have a flash', function() {
                should.not.exist(this.res.view.args.flash.error);
            });
        });
    });

    describe('logout', function() {
        describe('with no logged in user', function() {
            beforeEach(function() {
                controllers.logout(this.req, this.res);
            });

            shared.shouldRedirect('/');
        });

        describe('with a logged in user', function() {
            var user;

            beforeEach(function() {
                user = new User({ googleToken: 'test', displayName: 'test' });
                logInUser(this.req, user);
                controllers.logout(this.req, this.res);
            });

            shared.shouldRedirect('/');

            it('should unset the user', function() {
                should.not.exist(this.req.user);
            });
        });
    });

    describe('user controller', function() {
        describe('index action', function() {
            describe('with no logged in user', function() {
                beforeEach(function() {
                    controllers.user.index(this.req, this.res);
                });

                shared.shouldRedirect('/');
            });

            describe('with a logged in user', function() {
                var user;

                beforeEach(function() {
                    user = new User({ googleToken: 'test', displayName: 'test' });
                    logInUser(this.req, user);
                    controllers.user.index(this.req, this.res);
                });

                shared.shouldAllowAccess();
                shared.shouldRender('user');

                it('should pass the correct user arguments', function() {
                    should.exist(this.res.view.args.user);
                    should.exist(this.res.view.args.user.displayName);

                    this.res.view.args.user.displayName.should.equal(user.displayName);
                });
            });
        });
    });
});