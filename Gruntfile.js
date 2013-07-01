module.exports = function(grunt) {
	grunt.initConfig({
		env: {
			options: {
				// shared options hash
			},
			dev: {
				NODE_ENV: 'dev'
			}
		},
		watch: {
			scripts: {
				files: ['Gruntfile.js', 'server.js', 'lib/**/*.js', 'views/**/*.html'],
				tasks: ['jshint'],
				options: {
					interrupt: true
				}
			}
		},
		jshint: {
			all: ['Gruntfile.js', 'server.js', 'lib/**/*.js']
		},
		nodemon: {
			dev: {}
		},
		concurrent: {
			target: {
				tasks: ['watch', 'nodemon'],
				options: {
					logConcurrentOutput: true
				}
			}
		}
	});

	grunt.loadNpmTasks('grunt-contrib-watch');
	grunt.loadNpmTasks('grunt-contrib-jshint');
	grunt.loadNpmTasks('grunt-concurrent');
	grunt.loadNpmTasks('grunt-nodemon');
	grunt.loadNpmTasks('grunt-env');

	grunt.registerTask('default', ['env:dev', 'concurrent:target']);
};