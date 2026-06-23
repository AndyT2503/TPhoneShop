import { provideEnvironmentConfig } from '@tphone-shop.web/environment-config';
import { APP_BASE_HREF } from '@angular/common';
import { CommonEngine } from '@angular/ssr/node';
import express from 'express';
import cors from 'cors';
import { existsSync, readFileSync } from 'node:fs';
import { join } from 'node:path';
import bootstrap from './bootstrap.server';
import { AUTH_ROUTES } from '@tphone-shop.web/routing-config';

// The Express app is exported so that it can be used by serverless Functions.
export function app(): express.Express {
  const server = express();
  const distFolder = join(process.cwd(), 'dist/apps/shell/browser');
  const envConfig = JSON.parse(
    readFileSync(
      join(process.cwd(), 'dist/apps/shell/browser/config/env-config.json'),
      'utf8',
    ),
  );
  const indexHtml = existsSync(join(distFolder, 'index.original.html'))
    ? join(distFolder, 'index.original.html')
    : join(distFolder, 'index.html');

  server.use(cors());

  const commonEngine = new CommonEngine();

  server.set('view engine', 'html');
  server.set('views', distFolder);

  // Example Express Rest API endpoints
  // server.get('/api/**', (req, res) => { });
  // Serve static files from /browser
  server.get(
    '*.*',
    express.static(distFolder, {
      maxAge: '1y',
    }),
  );

  // All regular routes use the Angular engine
  server.get('*', (req, res, next) => {
    const CSR_ROUTES = [...Object.values(AUTH_ROUTES)];

    const isCSRRoute = CSR_ROUTES.some(
      (route) => req.path === route || req.path.startsWith(route + '/'),
    );

    if (isCSRRoute) {
      return res.sendFile(join(distFolder, 'index.html'));
    }

    const { protocol, originalUrl, baseUrl, headers } = req;

    commonEngine
      .render({
        bootstrap,
        documentFilePath: indexHtml,
        url: `${protocol}://${headers.host}${originalUrl}`,
        publicPath: distFolder,
        providers: [
          { provide: APP_BASE_HREF, useValue: baseUrl },
          provideEnvironmentConfig(envConfig),
        ],
      })
      .then((html) => res.send(html))
      .catch((err) => next(err));
  });

  return server;
}

function run(): void {
  const port = process.env['PORT'] || 4000;

  // Start up the Node server
  const server = app();
  server.listen(port, () => {
    console.log(`Node Express server listening on http://localhost:${port}`);
  });
}

run();

export default bootstrap;
