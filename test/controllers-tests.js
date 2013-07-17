var path = require('path'),
    controllers = require(path.join(process.cwd(), 'lib', 'controllers.js')),
    should = require('should'),
    shared = require(path.join(process.cwd(), 'test', 'helpers', 'controllers-test-helper.js')),
    request = shared.request,
    response = shared.response;

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
                user = shared.createTestUser();
                shared.logInUser(this.req, user);
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
                user = shared.createTestUser();
                shared.logInUser(this.req, user);
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
                shared.shouldRejectLoggedInUser(function() {
                    controllers.user.index(this.req, this.res);
                });
            });

            describe('with a logged in user', function() {
                var user;

                beforeEach(function() {
                    user = shared.createTestUser();
                    shared.logInUser(this.req, user);
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

        describe('update action', function() {
            describe('with no logged in user', function() {
                shared.shouldRejectLoggedInUser(function() {
                    controllers.user.update(this.req, this.res);
                });
            });

            describe('with a logged in user', function() {

            });
        });
    });
});