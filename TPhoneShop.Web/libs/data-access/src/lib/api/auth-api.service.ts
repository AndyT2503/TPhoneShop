import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { injectEnvironmentConfig } from '@tphone-shop.web/environment-config';
import {
  AuthResponse,
  ExternalLoginRequest,
  ForgotPasswordRequest,
  LoginRequest,
  RegisterRequest,
  ResetPasswordRequest,
  UserProfileResponse,
} from '../models';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root',
})
export class AuthAPIService {
  private readonly envConfig = injectEnvironmentConfig();
  private readonly httpClient = inject(HttpClient);

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.httpClient.post<AuthResponse>(
      `${this.envConfig.identityService}/api/auth/login`,
      request,
      {
        withCredentials: true,
      },
    );
  }

  logout(): Observable<object> {
    return this.httpClient.post<object>(
      `${this.envConfig.identityService}/api/auth/logout`,
      {},
      {
        withCredentials: true,
      },
    );
  }

  externalLogin(request: ExternalLoginRequest): Observable<AuthResponse> {
    return this.httpClient.post<AuthResponse>(
      `${this.envConfig.identityService}/api/auth/external-login`,
      request,
      {
        withCredentials: true,
      },
    );
  }

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.httpClient.post<AuthResponse>(
      `${this.envConfig.identityService}/api/auth/register`,
      request,
      {
        withCredentials: true,
      },
    );
  }

  resetPassword(request: ResetPasswordRequest): Observable<object> {
    return this.httpClient.post<object>(
      `${this.envConfig.identityService}/api/auth/reset-password`,
      request,
    );
  }

  forgotPassword(request: ForgotPasswordRequest): Observable<object> {
    return this.httpClient.post(
      `${this.envConfig.identityService}/api/auth/forgot-password`,
      request,
    );
  }

  refreshToken(): Observable<AuthResponse> {
    return this.httpClient.post<AuthResponse>(
      `${this.envConfig.identityService}/api/auth/refresh-token`,
      {},
      {
        withCredentials: true,
      },
    );
  }

  getUserProfile(): Observable<UserProfileResponse> {
    return this.httpClient.get<UserProfileResponse>(
      `${this.envConfig.identityService}/api/auth/me`,
    );
  }
}
