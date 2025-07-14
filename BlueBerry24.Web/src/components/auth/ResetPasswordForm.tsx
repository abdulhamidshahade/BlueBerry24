import { resetPasswordAction } from '../../lib/actions/auth-actions';
import { redirect } from 'next/navigation';

interface ResetPasswordFormProps {
  email: string;
  token: string;
}

export default function ResetPasswordForm({ email, token }: ResetPasswordFormProps) {
  async function handleResetPassword(formData: FormData) {
    'use server';
    
    const result = await resetPasswordAction(formData);
    
    if (result.success) {
      redirect('/auth/login?message=Password has been reset successfully. Please sign in with your new password.');
    } else {
      redirect(`/auth/reset-password?email=${encodeURIComponent(email)}&token=${encodeURIComponent(token)}&error=${encodeURIComponent(result.error || 'Failed to reset password')}`);
    }
  }

  return (
    <div className="card shadow">
      <div className="card-body p-4">
        <div className="text-center mb-4">
          <h2 className="card-title">
            <i className="bi bi-lock-fill me-2"></i>
            Reset Password
          </h2>
          <p className="text-muted">
            Enter your new password below.
          </p>
        </div>

        <form action={handleResetPassword}>
          <input type="hidden" name="email" value={email} />
          <input type="hidden" name="token" value={token} />
          
          <div className="mb-3">
            <label htmlFor="newPassword" className="form-label">
              <i className="bi bi-lock me-1"></i>
              New Password
            </label>
            <input
              type="password"
              className="form-control"
              id="newPassword"
              name="newPassword"
              placeholder="Enter your new password"
              required
              minLength={6}
            />
            <div className="form-text">
              Password must be at least 6 characters long.
            </div>
          </div>

          <div className="mb-3">
            <label htmlFor="confirmPassword" className="form-label">
              <i className="bi bi-lock-fill me-1"></i>
              Confirm New Password
            </label>
            <input
              type="password"
              className="form-control"
              id="confirmPassword"
              name="confirmPassword"
              placeholder="Confirm your new password"
              required
              minLength={6}
            />
          </div>

          <button type="submit" className="btn btn-primary w-100 mb-3">
            <i className="bi bi-check-circle me-2"></i>
            Reset Password
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