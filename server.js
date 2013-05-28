var port = 8888;

var express = require('express'),
	cons = require('consolidate'),
	swig = require('swig'),
	app = express();

app.engine('.html', cons.swig);
app.set('view engine', 'html');

swig.init({
	root: __dirname + '/views',
	allowError: true
});

app.set('views', __dirname + '/views/pages');
app.set('view options', { layout: false });

app.get('/', function(req, res) {
	res.render('index');
});

app.listen(port);
console.log('listening on port ' + port);