/// <binding BeforeBuild='sass, bundle, minify-css' />
'use strict';
 
var gulp = require('gulp');
var sass = require('gulp-sass');
var concatCss = require('gulp-concat-css');
var cleanCSS = require('gulp-clean-css');

sass.compiler = require('node-sass');

gulp.task('sass', function () {
  return gulp.src('./wwwroot/styles/scss/**/*.scss')
    .pipe(sass().on('error', sass.logError))
      .pipe(gulp.dest('./wwwroot/styles/css'));
});
 
gulp.task('sass:watch', function () {
  gulp.watch('./wwwroot/styles/*/*.scss', ['sass']);
});

gulp.task('bundle', function () {
    return gulp.src('./wwwroot/styles/css/**/*.css')
        .pipe(concatCss("styles/bundle.css"))
        .pipe(gulp.dest('./wwwroot/'));
});

gulp.task('minify-css', () => {
  return gulp.src('./wwwroot/styles/css/bundle.css')
    .pipe(cleanCSS({compatibility: 'ie8'}))
    .pipe(gulp.dest('./wwwroot/styles/css/'));
});