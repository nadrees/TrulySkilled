var models = require('./models');

function _render(req, res, name) {
    var flash = {};
    if (req.flash) {
        flash.error = req.flash('error');
    }

    res.render(name, { 
        flash: flash,
        user: req.user,
        token: req.session._csrf
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
        _requireLoggedInUser(req, res, function() {
            _render(req, res, 'user');
        });
    },
    update: function(req, res) {
        function _handleError(errMessage, args) {
            if (args !== undefined)
                req.logger.log('error', '(POST /user) - ' + errMessage, args);
            else
                req.logger.error('(POST /user) - ' + errMessage);

            req.flash('error', 'An error occured while updating your profile. Please try again.');
            _done();
        }

        function _done() {
            _render(req, res, 'user');
        }

        _requireLoggedInUser(req, res, function() {
            if (req.body && req.body.displayName) {
                req.user.displayName = req.body.displayName;
                req.user.save(function(err, newUser) {
                    if (err) _handleError(err);
                    else _done();
                });
            }
            else {
                _handleError('Invalid post params (%s)', JSON.stringify(req.body));
            }
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