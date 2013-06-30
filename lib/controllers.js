function index(req, res) {
    res.render('index', {
        flash: req.flash('error')
    });
}

module.exports = {
    index: index
};