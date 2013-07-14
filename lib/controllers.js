var models = require('./models');

function _render(req, res, name) {
    var flash = {};
    if (req.flash) {
        flash.error = req.flash('error');
    }

    res.render(name, { 
        flash: flash,
        user: req.user 
    });
}

function _requireLoggedInUser(req, res, loggedInFunction) {
    if (!req.isAuthenticated()) {
        req.flash('error', 'You must be logged in to view this page.');
        res.redirect('/');
    }
    else {
        loggedInFunction(req, res);
    }
}

var user = {
    index: function(req, res) {
        _requireLoggedInUser(req, res, function(req, res) {
            _render(req, res, 'user');
        });
    }
};

module.exports = {
    index: function(req, res) {
        _render(req, res, 'index');
    },
    logout: function(req, res) {
        req.logout();
        res.redirect('/');
    },
    user: user
};