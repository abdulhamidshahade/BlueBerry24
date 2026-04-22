'use client';

import { useState, useTransition } from 'react';
import { registerAction } from '../../lib/actions/auth-actions';

const rules = [
  {
    key: 'length',
    label: 'At least 6 characters',
    test: (v: string) => v.length >= 6,
  },
  {
    key: 'uppercase',
    label: 'One uppercase letter (A–Z)',
    test: (v: string) => /[A-Z]/.test(v),
  },
  {
    key: 'digit',
    label: 'One number (0–9)',
    test: (v: string) => /[0-9]/.test(v),
  },
  {
    key: 'special',
    label: 'One special character (!@#$…)',
    test: (v: string) => /[^A-Za-z0-9]/.test(v),
  },
];

interface RegisterFormProps {
  redirectTo?: string;
}

export default function RegisterForm({ redirectTo }: RegisterFormProps) {
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [error, setError] = useState('');
  const [isPending, startTransition] = useTransition();

  const allRulesPassed = rules.every((r) => r.test(password));
  const passwordsMatch = password === confirmPassword && confirmPassword !== '';

  function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();

    if (!allRulesPassed) {
      setError('Password does not meet all requirements.');
      return;
    }
    if (!passwordsMatch) {
      setError('Passwords do not match.');
      return;
    }

    setError('');
    const formData = new FormData(e.currentTarget);

    const email = (formData.get('email') as string) || '';

    startTransition(async () => {
      const result = await registerAction(formData);
      if (result) {
        let url = '/auth/confirm-email?email=' + encodeURIComponent(email);
        if (redirectTo && redirectTo.startsWith('/') && !redirectTo.startsWith('//')) {
          url += '&redirectTo=' + encodeURIComponent(redirectTo);
        }
        window.location.href = url;
      } else {
        setError('Registration failed. Please try again.');
      }
    });
  }

  return (
    <div className="card shadow">
      <div className="card-body p-4">
        <div className="text-center mb-4">
          <h2 className="card-title">
            <i className="bi bi-person-plus me-2"></i>
            Create Account
          </h2>
          <p className="text-muted">Join Berryfy and start shopping today!</p>
        </div>

        {error && (
          <div className="alert alert-danger py-2" role="alert">
            <i className="bi bi-exclamation-triangle me-2"></i>
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit}>
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

          {/* Password with live validation */}
          <div className="mb-3">
            <label htmlFor="password" className="form-label">
              <i className="bi bi-lock me-1"></i>
              Password
            </label>
            <input
              type="password"
              className={`form-control ${
                password === ''
                  ? ''
                  : allRulesPassed
                  ? 'is-valid'
                  : 'is-invalid'
              }`}
              id="password"
              name="password"
              placeholder="Create a password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />

            {/* Checklist */}
            <ul className="list-unstyled mt-2 mb-0 ps-1" style={{ fontSize: '0.85rem' }}>
              {rules.map((rule) => {
                const passed = rule.test(password);
                const touched = password !== '';
                return (
                  <li key={rule.key} className="d-flex align-items-center gap-2 mb-1">
                    {!touched ? (
                      <i className="bi bi-circle text-muted" />
                    ) : passed ? (
                      <i className="bi bi-check-circle-fill text-success" />
                    ) : (
                      <i className="bi bi-x-circle-fill text-danger" />
                    )}
                    <span className={!touched ? 'text-muted' : passed ? 'text-success' : 'text-danger'}>
                      {rule.label}
                    </span>
                  </li>
                );
              })}
            </ul>
          </div>

          {/* Confirm password */}
          <div className="mb-3">
            <label htmlFor="confirmPassword" className="form-label">
              <i className="bi bi-lock-fill me-1"></i>
              Confirm Password
            </label>
            <input
              type="password"
              className={`form-control ${
                confirmPassword === ''
                  ? ''
                  : passwordsMatch
                  ? 'is-valid'
                  : 'is-invalid'
              }`}
              id="confirmPassword"
              name="confirmPassword"
              placeholder="Confirm your password"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              required
            />
            {confirmPassword !== '' && !passwordsMatch && (
              <div className="invalid-feedback">Passwords do not match.</div>
            )}
            {confirmPassword !== '' && passwordsMatch && (
              <div className="valid-feedback">Passwords match!</div>
            )}
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

          <button
            type="submit"
            className="btn btn-primary w-100 mb-3"
            disabled={isPending || !allRulesPassed || !passwordsMatch}
          >
            {isPending ? (
              <>
                <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true" />
                Creating account…
              </>
            ) : (
              <>
                <i className="bi bi-person-plus me-2"></i>
                Create Account
              </>
            )}
          </button>

          <div className="text-center">
            <p className="mb-0">
              Already have an account?{' '}
              <a
                href={`/auth/login${redirectTo && redirectTo !== '/' ? `?redirectTo=${encodeURIComponent(redirectTo)}` : ''}`}
                className="text-decoration-none"
              >
                Sign in here
              </a>
            </p>
          </div>
        </form>
      </div>
    </div>
  );
}
