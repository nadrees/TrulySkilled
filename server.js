var express = require('express');

var app = express();

app.get('/', function(req, res) {
	res.end('Hello world!');
});

app.listen(8000);
console.log('Listening on port 8000');