import { User } from "./user";

export interface LoginRequest {
    email: string;
    password: string;
}

export interface ForgotPasswordRequest {
    email: string;
}

export interface ResetPasswordRequest {
    email: string;
    token: string;
    newPassword: string;
    confirmPassword: string;
}

export interface EmailConfirmationRequest {
    email: string;
    token: string;
}

export interface ResendConfirmationRequest {
    email: string;
}

export interface RegisterRequest {
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    password: string;
    confirmPassword: string;
}

export interface AuthResponse {
    isSuccess: boolean;
    statusCode: number;
    statusMessage: string;
    data?: {
        user: User;
        token: string;
    };
    errors?: string[];
}

export interface ApiResponse<T> {
    isSuccess: boolean;
    statusCode: number;
    statusMessage: string;
    data?: T;
    errors?: string[];
}

export interface AuthLog{
    user: User,
    token: string,

}