import esbuild from 'esbuild';
import http from 'http';
import { sassPlugin } from 'esbuild-sass-plugin';

esbuild.serve({
    servedir: 'www'
}, {
    entryPoints: ['src/index.tsx'],
    bundle: true,
    outfile: 'www/js/app.js',
    sourcemap: true,
    plugins: [sassPlugin()],
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

      // Forward each incoming request to esbuild
      const proxyReq = http.request(options, proxyRes => {
        // If esbuild returns "not found", send a custom 404 page
        if (proxyRes.statusCode === 404) {
          res.writeHead(404, { 'Content-Type': 'text/html' });
          res.end('<h1>A custom 404 page</h1>');
          return;
        }
  
        // Otherwise, forward the response from esbuild to the client
        res.writeHead(proxyRes.statusCode, proxyRes.headers);
        proxyRes.pipe(res, { end: true });
      });
  
      // Forward the body of the request to esbuild
      req.pipe(proxyReq, { end: true });
    }).listen(9000);
  });