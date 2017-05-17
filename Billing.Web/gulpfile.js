var gulp = require('gulp');
var clean = require('gulp-clean-css');
var concat = require('gulp-concat');
var minify = require('gulp-minify');
var uglify = require('gulp-uglify');
var rename = require('gulp-rename');
var source = ['scripts/*.js', 'scripts/*/*.js'];
var library = ['library/*.js'];
var style = 'styles/*.css';
var gutil = require('gulp-util');
var jshint = require('gulp-jshint');

gulp.task('default', function() {
    return gulp.src(source)
        .pipe(concat('app.js'))
        .pipe(uglify())
        .on('error', function (err) { gutil.log(gutil.colors.red('[Error]'), err.toString()); })
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
