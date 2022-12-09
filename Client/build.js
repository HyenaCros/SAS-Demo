import esbuild from 'esbuild';
import { sassPlugin } from 'esbuild-sass-plugin';

esbuild.build({
    entryPoints: ['src/index.tsx'],
    bundle: true,
    outfile: 'www/js/app.js',
    sourcemap: false,
    plugins: [sassPlugin()],
})