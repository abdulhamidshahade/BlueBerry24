import { loginAction } from '../../lib/actions/auth-actions';
import { redirect } from 'next/navigation';

interface LoginFormProps {
  redirectTo?: string;
}

export default function LoginForm({ redirectTo = '/' }: LoginFormProps) {
  async function handleLogin(formData: FormData) {
    'use server';
    
    const result = await loginAction(formData);
    
    if (result.success) {
      redirect(redirectTo);
    } else {
      if (result.emailNotConfirmed) {
        const email = formData.get('email') as string;
        redirect(`/auth/resend-confirmation?email=${encodeURIComponent(email)}&error=${encodeURIComponent(result.error || 'Email not confirmed')}`);
      } else {
        redirect(`/auth/login?error=${encodeURIComponent(result.error || 'Login failed')}`);
      }
    }
  }

  return (
    <div className="card shadow">
      <div className="card-body p-4">
        <div className="text-center mb-4">
          <h2 className="card-title">
            <i className="bi bi-person-circle me-2"></i>
            Sign In
          </h2>
          <p className="text-muted">Welcome back! Please sign in to your account.</p>
        </div>

        <form action={handleLogin}>
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
              placeholder="Enter your email"
              required
            />
          </div>

          <div className="mb-3">
            <label htmlFor="password" className="form-label">
              <i className="bi bi-lock me-1"></i>
              Password
            </label>
            <input
              type="password"
              className="form-control"
              id="password"
              name="password"
              placeholder="Enter your password"
              required
            />
          </div>

          <div className="mb-3 d-flex justify-content-between align-items-center">
            <div className="form-check">
              <input
                type="checkbox"
                className="form-check-input"
                id="rememberMe"
                name="rememberMe"
              />
              <label className="form-check-label" htmlFor="rememberMe">
                Remember me
              </label>
            </div>
            <div>
              <a href="/auth/forgot-password" className="text-decoration-none small">
                Forgot Password?
              </a>
            </div>
          </div>

          <button type="submit" className="btn btn-primary w-100 mb-3">
            <i className="bi bi-box-arrow-in-right me-2"></i>
            Sign In
          </button>

          <div className="text-center">
            <p className="mb-0">
              Don't have an account?{' '}
              <a href="/auth/register" className="text-decoration-none">
                Sign up here
              </a>
            </p>
          </div>
        </form>
      </div>
    </div>
  );
} 