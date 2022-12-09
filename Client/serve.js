import esbuild from 'esbuild';
import http from 'http';
import { sassPlugin } from 'esbuild-sass-plugin';

const apiPort = 3000;
const fileWatcherPort = 4000;

const log = (msg) => console.log(msg);
esbuild.serve({
    servedir: 'www'
}, {
    entryPoints: ['src/index.tsx'],
    bundle: true,
    outfile: 'www/js/app.js',
    sourcemap: true,
    plugins: [sassPlugin()],
    logLevel: 'info'
}).then(result => {
    // The result tells us where esbuild's local server is
    const {host, port} = result
  
    // Then start a proxy server on port 3000
    http.createServer((req, res) => {
      const options = {
        hostname: host,
        port: port,
        path: req.url,
        method: req.method,
        headers: req.headers,
      }

      if (options.method == 'GET' && options.headers.accept?.includes('text/html')) {
        options.path = '';
      }

      if (options.path.startsWith('/api/')) {
        options.port = options.path.startsWith('/api/Polling') ? fileWatcherPort : apiPort;
      }

      // Forward each incoming request to esbuild
      const proxyReq = http.request(options, proxyRes => {
  
        res.writeHead(proxyRes.statusCode, proxyRes.headers);
        proxyRes.pipe(res, { end: true });
      });
  
      // Forward the body of the request to esbuild
      req.pipe(proxyReq, { end: true });
    }).listen(9000);
  });