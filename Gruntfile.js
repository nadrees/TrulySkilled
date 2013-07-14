module.exports = function(grunt) {
    grunt.initConfig({
        env: {
            options: {
                // shared options hash
            },
            dev: {
                NODE_ENV: 'dev',
                PORT: '8888',
                DB_CONNECTION_STRING: 'mongodb://localhost/trulyskilled'
            },
            test: {
                NODE_ENV: 'test',
                PORT: '8000',
                DB_CONNECTION_STRING: 'mongodb://localhost/trulyskilled_test'
            },
            prod: {
                NODE_ENV: 'production',
                PORT: '80',
                DB_CONNECTION_STRING: 'mongodb://localhost/trulyskilled'
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
            all: ['Gruntfile.js', 'server.js', 'lib/**/*.js', 'test/**/*.js']
        },
        mochaTest: {
            test: {
                options: {
                    reporter: 'spec',
                    growl: true,
                    ignoreLeaks: false
                },
                src: ['test/**/*.js'],
            }
        }
    });

    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-contrib-jshint');
    grunt.loadNpmTasks('grunt-env');
    grunt.loadNpmTasks('grunt-mocha-test');

    grunt.registerTask('default', ['env:test', 'watch']);
    grunt.registerTask('server', 'Start the node server', function() {
        var done = this.async();
        require('./server.js').on('close', done);
    });
    grunt.registerTask('startServer', ['env:dev', 'server']);
};