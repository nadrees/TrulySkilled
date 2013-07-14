var mongoose = require('mongoose'),
    path = require('path'),
    should = require('should'),
    request = require('supertest'),
    superagent = require('superagent'),
	clearDb = require('mocha-mongoose')(process.env.DB_CONNECTION_STRING),
    passportMock = require(path.join(process.cwd(), 'test', 'mocks', 'passport-mock.js')),
    app = require(path.join(process.cwd(),'server.js'));

before(function() {
    passportMock.install(app);
});

after(function() {
    passportMock.uninstall(app);
});

afterEach(function() {
    passportMock.logout();
});

function attachCookiesAndFollowRedirect(res) {
    should.exist(res);
    should.exist(res.header);
    should.exist(res.header.location);

    var agent = superagent.agent();
    agent.saveCookies(res);

    var newReq = request(app).get(res.header.location);
    agent.attachCookies(newReq);
    return newReq;
}

module.exports = {
    logInUser: passportMock.login,
    logOutUser: passportMock.logout,
    attachCookiesAndFollowRedirect: attachCookiesAndFollowRedirect,
    shared: {
        shouldRejectAndRedirect: function() {
            it('should not allow annonymous access', function(done) {
                should.exist(this.req);
                this.req.expect(302, done);
            });

            it('should redirect to /', function(done) {
                should.exist(this.req);
                this.req.end(function(err, res) {
                    should.not.exist(err);
                    res.header.location.should.include('/');
                    done();
                });
            });

            it('should display an alert', function(done) {
                var agent = superagent.agent();
                should.exist(this.req);
                this.req.end(function(err, res) {
                    should.not.exist(err);

                    var newReq = attachCookiesAndFollowRedirect(res);
                    newReq.end(function(err, res) {
                        should.not.exist(err);
                        should.exist(res.text);

                        res.text.should.match(/<div class="alert">(.|\n|\r)*?<strong> *?Error: *?<\/strong>(.|\n|\r)*?<\/div>/);
                        done();
                    });
                });
            });
        }
    }
};