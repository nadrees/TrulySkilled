var path = require('path'),
    should = require('should'),
    models = require(path.join(process.cwd(), 'lib', 'models.js'));

describe('models', function() {
    describe("User model", function() {
        var user;

        beforeEach(function() {
            user = new models.User({ googleToken: 'test', displayName: 'test' });
        });

        it('should have an _id property', function() {
            should.exist(user._id);
        });

        it('should have a googleToken property', function() {
            should.exist(user.googleToken);
        });

        it('should have a displayName property', function() {
            should.exist(user.displayName);
        });

        describe('validations', function() {
            it('should save a valid record', function(done) {
                user.save(function(err, record) {
                    should.not.exist(err);
                    done();
                });
            });

            it('should require a displayName', function(done) {
                user.displayName = null;
                user.save(function(err, record) {
                    should.exist(err);
                    done();
                });
            });

            it('should require a unique googleToken', function(done) {
                user.save(function(err, record) {
                    should.not.exist(err);

                    var secondUser = new models.User({ googleToken: user.googleToken, displayName: 'test2' });
                    secondUser.save(function(err, record2) {
                        should.exist(err);
                        done();
                    });
                });
            });
        });
    });
});