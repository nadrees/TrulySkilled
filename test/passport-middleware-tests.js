var path = require('path'),
    should = require('should'),
    dbHelper = require('./helpers/db-helper.js'),
    passportMiddleware = require(path.join(process.cwd(), 'lib', 'passport-middleware.js')),
    User = require(path.join(process.cwd(), 'lib', 'models.js')).User;

describe('serializeUser', function() {
    describe('with a valid user', function() {
        var user;

        beforeEach(function(done) {
            user = new User({ googleToken: 'test', displayName: 'test' });
            user.save(function(err, user) {
                should.not.exist(err);
                done();
            });
        });

        it('should return the user\'s id', function() {
            passportMiddleware.serializeUser(user, function(err, id) {
                should.not.exist(err);

                id.should.equal(user._id);
            });
        });
    });

    describe('with an invalid user', function() {
        it('should have an error with a null user', function() {
            passportMiddleware.serializeUser(null, function(err, id) {
                should.exist(err);
            });
        });

        it('should have an error with a user with no id', function() {
            var user = new User();
            user._id = null;
            passportMiddleware.serializeUser(user, function(err, id) {
                should.exist(err);
            });
        });
    });
});

describe('deserializeUser', function() {
    describe('with a valid id', function() {
        var user;

        beforeEach(function(done) {
            user = new User({ googleToken: 'test', displayName: 'test' });
            user.save(function(err, user) {
                should.not.exist(err);
                done();
            });
        });

        it('should return the correct user', function(done) {
            passportMiddleware.deserializeUser(user._id, function(err, foundUser) {
                should.not.exist(err);
                should.exist(foundUser);

                foundUser._id.toString().should.equal(user._id.toString());
                foundUser.displayName.should.equal(user.displayName);
                done();
            });
        });
    });

    describe('with an invalid id', function() {
        it('should return an error', function(done) {
            passportMiddleware.deserializeUser(null, function(err, foundUser) {
                should.exist(err);
                done();
            });
        });
    });
});

describe('handleReturn', function() {
    var identifier = 'test',
        profile = {
            displayName: 'test'
        };

    describe('for a new user', function() {
        var err, user;

        beforeEach(function(done) {
            User.findOne({ 'googleToken': identifier }, '_id', function(foundErr, foundUser) {
                should.not.exist(foundErr);
                should.not.exist(foundUser);
                
                passportMiddleware.handleReturn(identifier, profile, function(retErr, retUser) {
                    err = retErr;
                    user = retUser;
                    done();
                });
            });
        });

        it('should not return an error', function() {
            should.not.exist(err);
        });

        it('should return the user', function() {
            should.exist(user);
            should.exist(user._id);
        });

        it('should save the user', function(done) {
            User.findOne({ '_id': user._id }, '_id', function(error, foundUser) {
                should.not.exist(error);
                should.exist(foundUser);

                foundUser._id.toString().should.equal(user._id.toString());
                done();
            });
        });
    });

    describe('for an existing user', function() {
        var err, user, returnedUser;

        beforeEach(function(done) {
            user = new User({ googleToken: 'test', displayName: 'test' });
            user.save(function(saveErr, savedUser) {
                should.not.exist(saveErr);

                passportMiddleware.handleReturn(user.googleToken, { displayName: user.displayName }, function(retErr, retUser) {
                    err = retErr;
                    returnedUser = retUser;
                    done();
                });
            });
        });

        it('should not return an error', function() {
            should.not.exist(err);
        });

        it('should return the user', function() {
            should.exist(returnedUser);
            should.exist(returnedUser._id);

            returnedUser._id.toString().should.equal(user._id.toString());
        });

        it('should not create a new user', function(done) {
            User.find({ identifier: user.identifier }, '_id', function(err, users) {
                should.not.exist(err);
                users.length.should.equal(1);
                done();
            });
        });
    });
});