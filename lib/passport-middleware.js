function handleReturn(identifier, profile, done) {
    console.log('OpenId.return called with identifier ' + identifier);
    console.log(JSON.stringify(profile));
    done(null, 1);
}

function serializeUser(user, done) {
    // TODO replace with db logic
    done(null, user);
}

function deserializeUser(id, done) {
    // TODO replace with db logic
    done(null, id);
}

module.exports = {
    handleReturn: handleReturn,
    serializeUser: serializeUser,
    deserializeUser: deserializeUser
};