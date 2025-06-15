'use server';

import { cookies } from 'next/headers';
import { redirect } from 'next/navigation';
import { AuthService } from '@/lib/services/auth/service';
import { LoginRequest, RegisterRequest } from '@/types/auth';
import { User } from '@/types/user';

export async function loginAction(formData: FormData) {
  const email = formData.get('email') as string;
  const password = formData.get('password') as string;

  if (!email || !password) {
    return {
      error: 'Email and password are required',
    };
  }

  try {
    const credentials: LoginRequest = { email, password };
    const response = await AuthService.login(credentials);

    if (response.isSuccess && response.data) {
      const cookieStore = await cookies();
      
      cookieStore.set('auth_token', response.data.token, {
        path: '/',
        httpOnly: true,
        secure: process.env.NODE_ENV === 'production',
        sameSite: 'lax',
        maxAge: 24 * 60 * 60, // 24 hours
      });

      cookieStore.set('user_info', JSON.stringify(response.data.user), {
        path: '/',
        httpOnly: true,
        secure: process.env.NODE_ENV === 'production',
        sameSite: 'lax',
        maxAge: 24 * 60 * 60, // 24 hours
      });

      

      return { success: true };
    } else {
      if (response.statusCode === 403) {
        return {
          error: response.statusMessage || 'Email not confirmed',
          emailNotConfirmed: true,
        };
      }
      
      return {
        error: response.statusMessage || 'Login failed',
      };
    }
  } catch (error) {
    console.error('Login error:', error);
    return {
      error: 'An unexpected error occurred',
    };
  }
}

export async function registerAction(formData: FormData) {
  const firstName = formData.get('firstName') as string;
  const lastName = formData.get('lastName') as string;
  const userName = formData.get('userName') as string;
  const email = formData.get('email') as string;
  const password = formData.get('password') as string;
  const confirmPassword = formData.get('confirmPassword') as string;

  if (!firstName || !lastName || !userName || !email || !password || !confirmPassword) {
    return {
      error: 'All fields are required',
    };
  }

  if (password !== confirmPassword) {
    return {
      error: 'Passwords do not match',
    };
  }

  try {
    const userData: RegisterRequest = {
      firstName,
      lastName,
      userName,
      email,
      password,
      confirmPassword,
    };

    const response = await AuthService.register(userData);

    //TODO: make here validations for register functionality
    // if (response.isSuccess) {
    //   return { success: true };
    // } else {
    //   return {
    //     error: response.statusMessage || 'Registration failed',
    //   };
    // }

    return true;

  } catch (error) {
    console.error('Registration error:', error);
    return {
      error: 'An unexpected error occurred',
    };
  }
}

export async function forgotPasswordAction(formData: FormData) {
  const email = formData.get('email') as string;

  if (!email) {
    return {
      error: 'Email is required',
    };
  }

  try {
    const response = await AuthService.forgotPassword({ email });

    if (response.isSuccess) {
      return { success: true };
    } else {
      return {
        error: response.statusMessage || 'Failed to process forgot password request',
      };
    }
  } catch (error) {
    console.error('Forgot password error:', error);
    return {
      error: 'An unexpected error occurred',
    };
  }
}

export async function resetPasswordAction(formData: FormData) {
  const email = formData.get('email') as string;
  const token = formData.get('token') as string;
  const newPassword = formData.get('newPassword') as string;
  const confirmPassword = formData.get('confirmPassword') as string;

  if (!email || !token || !newPassword || !confirmPassword) {
    return {
      error: 'All fields are required',
    };
  }

  if (newPassword !== confirmPassword) {
    return {
      error: 'Passwords do not match',
    };
  }

  try {
    const resetData = {
      email,
      token,
      newPassword,
      confirmPassword,
    };

    const response = await AuthService.resetPassword(resetData);

    if (response.isSuccess) {
      return { success: true };
    } else {
      return {
        error: response.statusMessage || 'Failed to reset password',
      };
    }
  } catch (error) {
    console.error('Reset password error:', error);
    return {
      error: 'An unexpected error occurred',
    };
  }
}

export async function confirmEmailAction(formData: FormData) {
  const email = formData.get('email') as string;
  const token = formData.get('token') as string;

  if (!email || !token) {
    return {
      error: 'Email and token are required',
    };
  }

  try {
    const confirmationData = {
      email,
      token,
    };

    const response = await AuthService.confirmEmail(confirmationData);

    if (response.isSuccess) {
      return { success: true };
    } else {
      return {
        error: response.statusMessage || 'Failed to confirm email',
      };
    }
  } catch (error) {
    console.error('Email confirmation error:', error);
    return {
      error: 'An unexpected error occurred',
    };
  }
}

export async function resendConfirmationAction(formData: FormData) {
  const email = formData.get('email') as string;

  if (!email) {
    return {
      error: 'Email is required',
    };
  }

  try {
    const response = await AuthService.resendConfirmation({ email });

    if (response.isSuccess) {
      return { success: true };
    } else {
      return {
        error: response.statusMessage || 'Failed to send confirmation email',
      };
    }
  } catch (error) {
    console.error('Resend confirmation error:', error);
    return {
      error: 'An unexpected error occurred',
    };
  }
}

export async function logoutAction() {
  const cookieStore = await cookies();
  const token = cookieStore.get('auth_token')?.value;

  if (token) {
    try {
      await AuthService.logout(token);
    } catch (error) {
      console.error('Logout API error:', error);
    }
  }

  cookieStore.delete('auth_token');
  cookieStore.delete('user_info');

  redirect('/');
}

export async function getCurrentUser(): Promise<User | null> {
  const cookieStore = await cookies();
  const token = cookieStore.get('auth_token')?.value;
  const userInfo = cookieStore.get('user_info')?.value;

  if (!token || !userInfo) {
    return null;
  }

  try {
    const user = JSON.parse(userInfo) as User;
    
    if (!user.roles) {
      user.roles = [];
    }
    
    // TODO: make here api call instead
    // const response = await AuthService.getCurrentUser(token);
    // if (!response.isSuccess) {
    //   return null;
    // }

    return user;
  } catch (error) {
    console.error('Error getting current user:', error);
    return null;
  }
}

export async function getAuthToken(): Promise<string | null> {
  const cookieStore = await cookies();
  return cookieStore.get('auth_token')?.value || null;
} 