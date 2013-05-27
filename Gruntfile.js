module.exports = function(grunt) {
	grunt.initConfig({
		watch: {
			scripts: {
				files: ['Gruntfile.js', 'server.js', 'src/**/*.js', 'views/**/*.html'],
				tasks: ['jshint'],
				options: {
					interrupt: true
				}
			}
		},
		jshint: {
			all: ['Gruntfile.js', 'server.js', 'src/**/*.js']
		}
	});

	grunt.loadNpmTasks('grunt-contrib-watch');
	grunt.loadNpmTasks('grunt-contrib-jshint');

	grunt.registerTask('default', ['watch']);
};