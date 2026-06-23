export interface AuthResponse {
  accessToken: string;
}

export interface ExternalLoginRequest {
  idToken: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest extends LoginRequest {
  fullName: string;
}

export interface ForgotPasswordRequest {
  email: string;
}

export interface ResetPasswordRequest {
  resetPasswordToken: string;
  newPassword: string;
}
