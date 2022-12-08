const esbuild = require('esbuild');
const sassPlugin = require('esbuild-sass-plugin').sassPlugin;

esbuild.build({
    entryPoints: ['src/index.tsx'],
    bundle: true,
    outfile: 'www/js/app.js',
    sourcemap: false,
    plugins: [sassPlugin()],
})