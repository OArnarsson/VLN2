var gulp = require('gulp'),
    browserSync = require('browser-sync');

const $ = require('gulp-load-plugins')();
const reload = browserSync.reload;

gulp.task('styles', () => {
  return gulp.src('Content/sass/theme.scss')
    .pipe($.plumber())
    .pipe($.sourcemaps.init())
    .pipe($.sass.sync({
        outputStyle: 'expanded',
        precision: 10,
        includePaths: ['.']
    }).on('error', $.sass.logError))
    .pipe($.autoprefixer({browsers: ['> 1%', 'last 2 versions', 'Firefox ESR']}))
    .pipe($.sourcemaps.write())
    //.pipe(plugins.minifyCss(options.minifyCss))
    .pipe(gulp.dest('Content'))
    .pipe(reload({stream: true}));
});

gulp.task('watcher', ['styles'], () => {
    gulp.watch('Content/sass/*.scss', ['styles']);
});

gulp.task('browsersync', ['styles'], () => {
    browserSync.init({
        open: 'external',
        proxy: 'localhost:3980',
        open: false,
        notify: false
    });
    gulp.start('watcher');
});

gulp.task('default', ['watcher'], () => {
});
