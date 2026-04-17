'use client';

import { useState, useTransition } from 'react';
import { resetPasswordAction } from '../../lib/actions/auth-actions';

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

interface ResetPasswordFormProps {
  email: string;
  token: string;
}

export default function ResetPasswordForm({ email, token }: ResetPasswordFormProps) {
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [error, setError] = useState('');
  const [errors, setErrors] = useState<string[]>([]);
  const [isPending, startTransition] = useTransition();

  const allRulesPassed = rules.every((r) => r.test(password));
  const passwordsMatch = password === confirmPassword && confirmPassword !== '';

  function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();

    if (!allRulesPassed) {
      setError('Password does not meet all requirements.');
      setErrors([]);
      return;
    }
    if (!passwordsMatch) {
      setError('Passwords do not match.');
      setErrors([]);
      return;
    }

    setError('');
    setErrors([]);

    const formData = new FormData(e.currentTarget);

    startTransition(async () => {
      const result = await resetPasswordAction(formData);

      if (result.success) {
        window.location.href =
          '/auth/login?message=' +
          encodeURIComponent('Password has been reset successfully. Please sign in with your new password.');
      } else {
        if (result.errors && result.errors.length > 1) {
          setErrors(result.errors);
          setError('');
        } else {
          setError(result.error || 'Failed to reset password. Please try again.');
          setErrors([]);
        }
      }
    });
  }

  return (
    <div className="card shadow">
      <div className="card-body p-4">
        <div className="text-center mb-4">
          <h2 className="card-title">
            <i className="bi bi-lock-fill me-2"></i>
            Reset Password
          </h2>
          <p className="text-muted">Enter your new password below.</p>
        </div>

        {error && (
          <div className="alert alert-danger py-2" role="alert">
            <i className="bi bi-exclamation-triangle me-2"></i>
            {error}
          </div>
        )}

        {errors.length > 1 && (
          <div className="alert alert-danger py-2" role="alert">
            <i className="bi bi-exclamation-triangle me-2"></i>
            <strong>Please fix the following:</strong>
            <ul className="mb-0 mt-1 ps-3">
              {errors.map((e, i) => (
                <li key={i}>{e}</li>
              ))}
            </ul>
          </div>
        )}

        <form onSubmit={handleSubmit}>
          <input type="hidden" name="email" value={email} />
          <input type="hidden" name="token" value={token} />

          {/* New password with live validation */}
          <div className="mb-3">
            <label htmlFor="newPassword" className="form-label">
              <i className="bi bi-lock me-1"></i>
              New Password
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
              id="newPassword"
              name="newPassword"
              placeholder="Enter your new password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              disabled={isPending}
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
              Confirm New Password
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
              placeholder="Confirm your new password"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              required
              disabled={isPending}
            />
            {confirmPassword !== '' && !passwordsMatch && (
              <div className="invalid-feedback">Passwords do not match.</div>
            )}
            {confirmPassword !== '' && passwordsMatch && (
              <div className="valid-feedback">Passwords match!</div>
            )}
          </div>

          <button
            type="submit"
            className="btn btn-primary w-100 mb-3"
            disabled={isPending || !allRulesPassed || !passwordsMatch}
          >
            {isPending ? (
              <>
                <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true" />
                Resetting password…
              </>
            ) : (
              <>
                <i className="bi bi-check-circle me-2"></i>
                Reset Password
              </>
            )}
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
