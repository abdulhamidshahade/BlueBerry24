import { forgotPasswordAction } from '@/lib/actions/auth-actions';
import { redirect } from 'next/navigation';

export default function ForgotPasswordForm() {
  async function handleForgotPassword(formData: FormData) {
    'use server';
    
    const result = await forgotPasswordAction(formData);
    
    if (result.success) {
      redirect('/auth/forgot-password?message=Password reset instructions have been sent to your email address.');
    } else {
      redirect(`/auth/forgot-password?error=${encodeURIComponent(result.error || 'Failed to process request')}`);
    }
  }

  return (
    <div className="card shadow">
      <div className="card-body p-4">
        <div className="text-center mb-4">
          <h2 className="card-title">
            <i className="bi bi-key me-2"></i>
            Forgot Password
          </h2>
          <p className="text-muted">
            Enter your email address and we'll send you a link to reset your password.
          </p>
        </div>

        <form action={handleForgotPassword}>
          <div className="mb-3">
            <label htmlFor="email" className="form-label">
              <i className="bi bi-envelope me-1"></i>
              Email Address
            </label>
            <input
              type="email"
              className="form-control"
              id="email"
              name="email"
              placeholder="Enter your email address"
              required
            />
          </div>

          <button type="submit" className="btn btn-primary w-100 mb-3">
            <i className="bi bi-envelope-arrow-up me-2"></i>
            Send Reset Link
          </button>

          <div className="text-center">
            <p className="mb-0">
              Remember your password?{' '}
              <a href="/auth/login" className="text-decoration-none">
                Sign in here
              </a>
            </p>
          </div>
        </form>
      </div>
    </div>
  );
} 