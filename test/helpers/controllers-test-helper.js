var path = require('path'),
    should = require('should'),
    User = require(path.join(process.cwd(), 'lib', 'models.js')).User;

function logInUser(req, user) {
    req.user = user;
}
function createTestUser() {
    return new User({ googleToken: 'test', displayName: 'test' });
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

function shouldAllowAccess() {
    it('should allow access', function() {
        should.exist(this.res.view);
        should.exist(this.res.view.name);
        should.exist(this.res.view.args);
    });
}
function shouldRender(name) {
    it('should render ' + name, function() {
        this.res.view.name.should.equal(name);
    });
}
function shouldRedirect(name) {
    it('should redirect to ' + name, function() {
        should.not.exist(this.res.view);
        should.exist(this.res.redirectLocation);
        this.res.redirectLocation.should.equal(name);
    });
}
function shouldRejectLoggedInUser(setupFunction) {
    beforeEach(setupFunction);

    shouldRedirect('/');
}

module.exports = {
    shouldAllowAccess: shouldAllowAccess,
    shouldRejectLoggedInUser: shouldRejectLoggedInUser,
    shouldRender: shouldRender,
    shouldRedirect: shouldRedirect,
    logInUser: logInUser,
    createTestUser: createTestUser,
    request: request,
    response: response
};