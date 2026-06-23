import type { ModuleFederationConfig } from '@nx/module-federation';

const config: ModuleFederationConfig = {
  name: 'auth',
  exposes: {
    './Routes': 'apps/auth/src/app/remote-entry/entry.routes.ts',
  },
  additionalShared: [
    {
      libraryName: 'firebase/auth',
      sharedConfig: {
        singleton: true,
        strictVersion: false,
        requiredVersion: '12.15.0',
      },
    },
    {
      libraryName: 'firebase/app',
      sharedConfig: {
        singleton: true,
        strictVersion: false,
        requiredVersion: '12.15.0',
      },
    },
  ],
};

/**
 * Nx requires a default export of the config to allow correct resolution of the module federation graph.
 **/
export default config;
