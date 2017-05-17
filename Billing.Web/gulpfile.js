var gulp = require('gulp');
var clean = require('gulp-clean-css');
var concat = require('gulp-concat');
var minify = require('gulp-minify');
var uglify = require('gulp-uglify');
var rename = require('gulp-rename');
var source = ['scripts/app.js', 'scripts/controllers/*.js', 'scripts/controllers/ReportCtrl/*.js', 'scripts/directives/*.js', 'scripts/services/*.js'];
var library = ['library/*.js'];
var style = 'styles/*.css';
var jshint = require('gulp-jshint');
var ngAnnotate = require('gulp-ng-annotate');

gulp.task('default', function() {
    return gulp.src(source)
        .pipe(concat('app.js'))
        .pipe(ngAnnotate())
        .pipe(uglify())
        .pipe(rename('app.min.js'))
        .pipe(gulp.dest('build'));
});

gulp.task('lib', function() {
    return gulp.src(library)
        .pipe(concat('lib.min.js'))
        .pipe(gulp.dest('build'))
});

gulp.task('css', function() {
    return gulp.src(style)
        .pipe(concat('app.css'))
        .pipe(clean('app.css'))
        .pipe(rename('app.min.css'))
        .pipe(gulp.dest('build'))
});

gulp.task('jshint', function() {

    gulp.src(['scripts/app.js', 'scripts/*/*.js'])
        .pipe(jshint())
        .pipe(jshint.reporter('gulp-jshint-file-reporter', { filename: './jsCheck.txt' }));
});
