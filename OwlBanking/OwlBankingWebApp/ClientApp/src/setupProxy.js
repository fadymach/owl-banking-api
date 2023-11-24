const { createProxyMiddleware } = require('http-proxy-middleware');
const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:20464';

const context =  [
  "/weatherforecast",
];

module.exports = function (app) {
  app.use(
      '/api',
      createProxyMiddleware({
        target: 'http://localhost:7136', // Replace with the actual URL of your backend API
        changeOrigin: true,
      })
  );
};

// module.exports = function(app) {
//   const appProxy = createProxyMiddleware(context, {
//     target: target,
//     secure: false,
//     headers: {
//       Connection: 'Keep-Alive'
//     }
//   });
//
//   app.use(appProxy);
// };
