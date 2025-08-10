const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7097';

const prox = { target: target, secure: false };
const PROXY_CONFIG = {
  "/Documentation/Lang": prox,
  "/Search/CheckSearchCount": prox,
  "/Search/RunSearch": prox
}

module.exports = PROXY_CONFIG;
