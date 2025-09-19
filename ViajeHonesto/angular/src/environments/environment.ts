import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

const oAuthConfig = {
  issuer: 'https://localhost:44383/',
  redirectUri: baseUrl,
  clientId: 'ViajeHonesto_App',
  responseType: 'code',
  scope: 'offline_access ViajeHonesto',
  requireHttps: true,
};

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'ViajeHonesto',
  },
  oAuthConfig,
  apis: {
    default: {
      url: 'https://localhost:44383',
      rootNamespace: 'ViajeHonesto',
    },
    AbpAccountPublic: {
      url: oAuthConfig.issuer,
      rootNamespace: 'AbpAccountPublic',
    },
  },
} as Environment;
