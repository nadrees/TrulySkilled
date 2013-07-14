var models = require('./models'),
    User = models.User;

function handleReturn(identifier, profile, done) {
    User.findOne({ 'googleToken': identifier }, '_id displayName', function(err, user) {
        if (err) done(err, false);

        if (user !== null) {
            done(null, user);
        }
        else {
            user = new User({ googleToken: identifier, displayName: profile.displayName });
            user.save(function(err) {
                if (err) done(err, null);
                else done(null, user);
            });
        }
    });
}

function serializeUser(user, done) {
    if (user && user._id) done(null, user._id);
    else done('Object "user" was null or had no _id field', null);
}

function deserializeUser(id, done) {
    User.findOne({ '_id': id }, 'displayName', function(err, user) {
        if (err) done(err, null);
        else if (user === null) done('No user with id ' + id + ' could be found', null);
        else done(null, user);
    });
}

module.exports = {
    handleReturn: handleReturn,
    serializeUser: serializeUser,
    deserializeUser: deserializeUser
};