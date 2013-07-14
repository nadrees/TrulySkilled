var mongoose = require('mongoose'),
    clearDb = require('mocha-mongoose')(process.env.DB_CONNECTION_STRING);

beforeEach(function(done) {
    if (mongoose.connection.db) {
        done();
    }
    else {
        mongoose.connect(dbURI, done);
    }
});