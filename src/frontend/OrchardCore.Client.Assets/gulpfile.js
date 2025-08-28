const rename = require('gulp-rename');
const uglify = require('gulp-uglify');
const sourcemaps = require('gulp-sourcemaps');
const csso = require('gulp-csso');
const replace = require('gulp-replace');
const del = require('del');
const { series, parallel, src, dest } = require('gulp');
const sass = require('gulp-sass')(require('sass'));

function clean() {
    return del(['./wwwroot/**', '!./wwwroot/.placeholder']);
}

function assets() {
    return src('./pack/assets/**')
        .pipe(dest('./wwwroot/assets/'));
}

function gdsAssets() {
    return src('./node_modules/govuk-frontend/dist/govuk/assets/**')
        .pipe(dest('./wwwroot/assets/'));
}

function jquery() {
    return src('./node_modules/jquery/dist/**')
        .pipe(dest('./wwwroot/javascripts/'));
}

//function gdsStyles() {
//    return src('./node_modules/govuk-frontend/dist/govuk/all.scss')
//        .pipe(sourcemaps.init())
//        .pipe(sass())
//        .pipe(sourcemaps.write())
//        .pipe(replace(/assets\//g, 'contents/assets/'))
//        .pipe(dest('./wwwroot/stylesheets/'))
//        .pipe(csso({ restructure: false }))
//        .pipe(rename(function (path) {
//            return {
//                dirname: path.dirname,
//                basename: path.basename,
//                extname: '.min.css'
//            }
//        }))
//        .pipe(dest('./wwwroot/stylesheets/'));
//}

function gdsStyles() {
    return src('./node_modules/govuk-frontend/dist/govuk/govuk-frontend.min.css')
        .pipe(replace(/assets\//g, 'contents/assets/'))
        .pipe(dest('./wwwroot/stylesheets/'));
}

function gdsScripts() {
    return src('./node_modules/govuk-frontend/dist/govuk/govuk-frontend.min.js')
        .pipe(dest('./wwwroot/javascripts/'));
}

function styles() {
    return src('./pack/*.scss')
        .pipe(sourcemaps.init())
        .pipe(sass())
        .pipe(sourcemaps.write())
        .pipe(dest('./wwwroot/stylesheets/'))
        .pipe(csso({ restructure: false }))
        .pipe(rename(function (path) {
            return {
                dirname: path.dirname,
                basename: path.basename,
                extname: '.min.css'
            }
        }))
        .pipe(dest('./wwwroot/stylesheets/'));
};

function scripts() {
    return src('./pack/*.js')
        .pipe(dest('./wwwroot/javascripts/'))
        .pipe(uglify())
        .pipe(rename({ extname: '.min.js' }))
        .pipe(dest('./wwwroot/javascripts/'));
}

const build = series(clean, assets, gdsAssets, jquery, gdsScripts, gdsStyles, parallel(styles, scripts));
module.exports = { clean, build };
