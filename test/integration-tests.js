var request = require('supertest'),
    path = require('path'),
    should = require('should'),
    app = require(path.join(process.cwd(),'server.js')),
    helpers = require(path.join(process.cwd(), 'test', 'helpers', 'integration-test-helper.js')),
    dbHelpers = require(path.join(process.cwd(), 'test', 'helpers', 'db-helper.js')),
    logInUser = helpers.logInUser,
    shared = helpers.shared,
    User = require(path.join(process.cwd(), 'lib', 'models.js')).User;

describe('integration tests', function() {

    beforeEach(function() {
        this.user = new User({_id:1, googleToken: 'test'});
    });


    describe('GET /', function() {
        it('should allow annonymous access', function(done) {
            var req = request(app).get('/');
            req.expect(200, done);
        });

        it('should allow authenticated access', function(done) {
            logInUser(this.user);
            var req = request(app).get('/');
            req.expect(200, done);
        });
    });

    describe('GET /user', function() {
        describe('with no logged in user', function() {

            beforeEach(function() {
                this.req = request(app).get('/user');
            });

            shared.shouldRejectAndRedirect();
        });

        describe('with logged in user', function() {
            beforeEach(function() {
                logInUser(this.user);
                this.req = request(app).get('/user');
            });

            it('should allow access', function(done) {
                this.req.expect(200, done);
            });
        });
    });
});