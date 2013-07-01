var models = require('./models');

function index(req, res) {
    res.render('index');
}

module.exports = {
    index: index
};