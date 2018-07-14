/// <binding Clean='clean' />
"use strict";

var gulp = require("gulp");
var browserSync = require('browser-sync').create();
var sass = require('gulp-sass');
var rimraf = require("rimraf");
var concat = require("gulp-concat");
var cssmin = require("gulp-cssmin");
var uglify = require("gulp-uglify");
var merge = require("gulp-merge");
var paths = {
    webroot: "./wwwroot/"
};

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";

// Dependency Dirs
var deps = {
    "jquery": {
        "dist/*": ""
    },
    "bootstrap": {
        "dist/**/*": ""
    },
    // ...
};

gulp.task("scripts", function () {
    var streams = [];
    for (var prop in deps) {
        console.log("Prepping Scripts for: " + prop);
        for (var itemProp in deps[prop]) {
            streams.push(gulp.src("node_modules/" + prop + "/" + itemProp)
                .pipe(gulp.dest("wwwroot/vendor/" + prop + "/" + deps[prop][itemProp])));
        }
    }
    return merge(streams);
});

// Compile sass into CSS & auto-inject into browsers
// ** maybe you have to update the referenced folders
gulp.task('sass', function () {
    return gulp.src(['node_modules/bootstrap/scss/bootstrap.scss', 'src/scss/*.scss'])
        .pipe(sass())
        .pipe(gulp.dest("src/css"))
        .pipe(browserSync.stream());
});

// Move the javascript files into our /src/js folder
// ** maybe you have to update the referenced folders  
gulp.task('js', function () {
    return gulp.src(['node_modules/bootstrap/dist/js/bootstrap.min.js', 'node_modules/jquery/dist/jquery.min.js', 'node_modules/popper.js/dist/umd/popper.min.js'])
        .pipe(gulp.dest("src/js"))
        .pipe(browserSync.stream());
});

// Static Server + watching scss/html files
// ** maybe you have to update the referenced folders  
gulp.task('serve', ['sass'], function () {

    browserSync.init({
        server: "./src"
    });

    gulp.watch(['node_modules/bootstrap/scss/bootstrap.scss', 'src/scss/*.scss'], ['sass']);
    gulp.watch("src/*.html").on('change', browserSync.reload);
});

// der Default-Task wird ausgeführt wenn beim Start kein expliziter Task angegeben wird
gulp.task('default', ['js', 'serve']);