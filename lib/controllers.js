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

module.exports = {
    index: function(req, res) {
        _render(req, res, 'index');
    },
    logout: function(req, res) {
        req.logout();
        res.redirect('/');
    }
};