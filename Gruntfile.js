module.exports = function(grunt) {
	grunt.initConfig({
		env: {
			options: {
				// shared options hash
			},
			dev: {
				NODE_ENV: 'dev',
				PORT: '8888'
			}
		}, 
		watch: {
			scripts: {
				files: ['Gruntfile.js', 'server.js', 'lib/**/*.js', 'views/**/*.html', 'test/**/*.js'],
				tasks: ['jshint', 'mochaTest'],
				options: {
					interrupt: true
				}
			}
		},
		jshint: {
			all: ['Gruntfile.js', 'server.js', 'lib/**/*.js']
		},
		mochaTest: {
			test: {
				options: {
					reporter: 'dot',
					growl: true,
					ignoreLeaks: false
				},
				src: 'test/integration-tests.js'
			}
		}
	});

	grunt.loadNpmTasks('grunt-contrib-watch');
	grunt.loadNpmTasks('grunt-contrib-jshint');
	grunt.loadNpmTasks('grunt-env');
	grunt.loadNpmTasks('grunt-mocha-test');

	grunt.registerTask('default', ['env:dev', 'mochaTest', 'watch']);
};