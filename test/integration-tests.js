var request = require('supertest'),
    superagent = require('superagent'),
    path = require('path'),
    app = require(path.join(process.cwd(),'server.js')),
    passportMock = require(path.join(process.cwd(), 'test', 'passport-mock.js'));

function logInUser(app) {
    passportMock(app, {
        passAuthentication: true,
        userId: 1
    });
    request(app)
        .get('mock/login')
        .end(function(err, result) {
            if (!err) {
                agent.saveCookies(result.res);
            }
            else {
                return err;
            }
        });
}

describe('GET /', function() {
    var agent = superagent.agent();

    it('should allow annonymous access', function(done) {
        var req = request(app).get('/');
        req.expect(200, done);
    });

    it('should allow authenticated access', function(done) {
        var err = logInUser(app);
        if (err) 
            done(err);
        else {
            var req = request(app).get('/');
            agent.attachCookies(req);
            req.expect(200, done);
        }
    });
});