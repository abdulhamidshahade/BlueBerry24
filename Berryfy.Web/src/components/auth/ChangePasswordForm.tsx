'use client';

import { useState, useTransition } from 'react';
import { changePasswordAction } from '../../lib/actions/auth-actions';

export default function ChangePasswordForm() {
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [isPending, startTransition] = useTransition();

  function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    setError('');
    setSuccess('');
    const formData = new FormData(e.currentTarget);

    startTransition(async () => {
      const result = await changePasswordAction(formData);
      if ('success' in result && result.success) {
        setSuccess('Password changed successfully!');
        (e.target as HTMLFormElement).reset();
      } else {
        setError((result as { error: string }).error || 'Failed to change password');
      }
    });
  }

  return (
    <form onSubmit={handleSubmit}>
      {error && (
        <div className="alert alert-danger" role="alert">
          <i className="bi bi-exclamation-circle me-2"></i>
          {error}
        </div>
      )}
      {success && (
        <div className="alert alert-success" role="alert">
          <i className="bi bi-check-circle me-2"></i>
          {success}
        </div>
      )}

      <div className="mb-3">
        <label htmlFor="currentPassword" className="form-label fw-semibold">
          Current Password
        </label>
        <input
          id="currentPassword"
          name="currentPassword"
          type="password"
          className="form-control"
          required
          autoComplete="current-password"
        />
      </div>

      <div className="mb-3">
        <label htmlFor="newPassword" className="form-label fw-semibold">
          New Password
        </label>
        <input
          id="newPassword"
          name="newPassword"
          type="password"
          className="form-control"
          required
          autoComplete="new-password"
          minLength={6}
        />
      </div>

      <div className="mb-3">
        <label htmlFor="confirmNewPassword" className="form-label fw-semibold">
          Confirm New Password
        </label>
        <input
          id="confirmNewPassword"
          name="confirmNewPassword"
          type="password"
          className="form-control"
          required
          autoComplete="new-password"
          minLength={6}
        />
      </div>

      <div className="d-flex gap-2">
        <button type="submit" className="btn btn-primary" disabled={isPending}>
          {isPending ? (
            <>
              <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
              Changing...
            </>
          ) : (
            <>
              <i className="bi bi-key me-1"></i>
              Change Password
            </>
          )}
        </button>
        <a href="/profile" className="btn btn-outline-secondary">
          Cancel
        </a>
      </div>
    </form>
  );
}
