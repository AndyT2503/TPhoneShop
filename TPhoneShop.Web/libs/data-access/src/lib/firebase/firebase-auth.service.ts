import { Injectable } from '@angular/core';
import { injectEnvironmentConfig } from '@tphone-shop.web/environment-config';
import { getApps, initializeApp } from 'firebase/app';
import {
  Auth,
  getAuth,
  GoogleAuthProvider,
  signInWithPopup,
  signOut,
} from 'firebase/auth';

@Injectable({
  providedIn: 'root',
})
export class FirebaseAuthService {
  private readonly envConfig = injectEnvironmentConfig();
  private auth!: Auth;

  async signInWithGoogle(): Promise<string> {
    this.auth = this.getAuthInstance();
    const provider = new GoogleAuthProvider();
    provider.setCustomParameters({
      prompt: 'select_account',
    });

    const result = await signInWithPopup(this.auth, provider);
    return await result.user.getIdToken();
  }

  private getAuthInstance() {
    if (!this.auth) {
      const app = this.getFirebaseApp();
      return getAuth(app);
    }
    return this.auth;
  }

  private getFirebaseApp() {
    return getApps().length
      ? getApps()[0]
      : initializeApp(this.envConfig.firebase);
  }
}
