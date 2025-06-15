import { registerAction } from '@/lib/actions/auth-actions';
import { redirect } from 'next/navigation';

export default function RegisterForm() {
  async function handleRegister(formData: FormData) {
    'use server';
    
    const result = await registerAction(formData);
    
    if (result) {
      redirect('/auth/login?message=Registration successful! Please check your email to confirm your account before signing in.');
    } else {
      redirect(`/auth/register?error=${encodeURIComponent(result || 'Registration failed')}`);
    }
  }

  return (
    <div className="card shadow">
      <div className="card-body p-4">
        <div className="text-center mb-4">
          <h2 className="card-title">
            <i className="bi bi-person-plus me-2"></i>
            Create Account
          </h2>
          <p className="text-muted">Join BlueBerry24 and start shopping today!</p>
        </div>

        <form action={handleRegister}>
          <div className="row">
            <div className="col-md-6 mb-3">
              <label htmlFor="firstName" className="form-label">
                <i className="bi bi-person me-1"></i>
                First Name
              </label>
              <input
                type="text"
                className="form-control"
                id="firstName"
                name="firstName"
                placeholder="Enter your first name"
                required
              />
            </div>
            <div className="col-md-6 mb-3">
              <label htmlFor="lastName" className="form-label">
                <i className="bi bi-person me-1"></i>
                Last Name
              </label>
              <input
                type="text"
                className="form-control"
                id="lastName"
                name="lastName"
                placeholder="Enter your last name"
                required
              />
            </div>
          </div>

          <div className="mb-3">
            <label htmlFor="userName" className="form-label">
              <i className="bi bi-at me-1"></i>
              Username
            </label>
            <input
              type="text"
              className="form-control"
              id="userName"
              name="userName"
              placeholder="Choose a username"
              required
            />
          </div>

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
              placeholder="Create a password"
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
              Confirm Password
            </label>
            <input
              type="password"
              className="form-control"
              id="confirmPassword"
              name="confirmPassword"
              placeholder="Confirm your password"
              required
              minLength={6}
            />
          </div>

          <div className="mb-3 form-check">
            <input
              type="checkbox"
              className="form-check-input"
              id="agreeTerms"
              required
            />
            <label className="form-check-label" htmlFor="agreeTerms">
              I agree to the{' '}
              <a href="/terms" className="text-decoration-none">
                Terms of Service
              </a>{' '}
              and{' '}
              <a href="/privacy" className="text-decoration-none">
                Privacy Policy
              </a>
            </label>
          </div>

          <button type="submit" className="btn btn-primary w-100 mb-3">
            <i className="bi bi-person-plus me-2"></i>
            Create Account
          </button>

          <div className="text-center">
            <p className="mb-0">
              Already have an account?{' '}
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