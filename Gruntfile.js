module.exports = function(grunt) {
	grunt.initConfig({
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
		}
	});

	grunt.loadNpmTasks('grunt-contrib-watch');
	grunt.loadNpmTasks('grunt-contrib-jshint');

	grunt.registerTask('default', ['watch']);
};