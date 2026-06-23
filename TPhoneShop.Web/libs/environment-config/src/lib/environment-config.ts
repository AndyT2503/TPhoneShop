export interface EnvironmentConfig {
  identityService: string;
  commerceService: string;
  notificationService: string;
  firebase: {
    apiKey: string;
    authDomain: string;
    projectId: string;
    storageBucket: string;
    messagingSenderId: string;
    appId: string;
  };
}
