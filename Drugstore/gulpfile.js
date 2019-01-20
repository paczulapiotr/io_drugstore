/// <binding BeforeBuild='clean-css, sass, bundle, minify-css, load-bootstrap' ProjectOpened='sass:watch' />
'use strict';
 
var gulp = require('gulp');
var sass = require('gulp-sass');
var concatCss = require('gulp-concat-css');
var rename = require('gulp-rename');
var cleanCSS = require('gulp-clean-css');
var clean = require('gulp-clean');

sass.compiler = require('node-sass');


gulp.task('load-bootstrap', function () {
    return gulp.src('./node_modules/bootstrap/dist/css/bootstrap.min.css')
            .pipe(gulp.dest('./wwwroot/styles'))
    });

gulp.task('clean-css', function () {
    return gulp.src('./wwwroot/styles/**/*.css', { read: false })
        .pipe(clean());
});

gulp.task('sass', function () {
  return gulp.src('./wwwroot/styles/scss/**/*.scss')
    .pipe(sass().on('error', sass.logError))
      .pipe(gulp.dest('./wwwroot/styles/css'));
});
 
gulp.task('bundle', function () {
    return gulp.src('./wwwroot/styles/css/**/*.css')
        .pipe(concatCss("styles/bundle.css"))
        .pipe(gulp.dest('./wwwroot/'));
});

gulp.task('minify-css', () => {
  return gulp.src('./wwwroot/styles/bundle.css')
	.pipe(cleanCSS({compatibility: 'ie8'}))
	.pipe(rename({suffix: '.min'}))
	.pipe(gulp.dest('./wwwroot/styles'));
});

gulp.task('sass:watch', function () {
    gulp.watch('./wwwroot/styles/scss/**/*.scss', gulp.series(['sass', 'bundle', 'minify-css']));
});