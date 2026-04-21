'use client';

import { useState, useTransition } from 'react';
import { updateProfileAction } from '../../lib/actions/auth-actions';
import { User } from '../../types/user';

interface EditProfileFormProps {
  user: User;
}

export default function EditProfileForm({ user }: EditProfileFormProps) {
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [isPending, startTransition] = useTransition();

  function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    setError('');
    setSuccess('');
    const formData = new FormData(e.currentTarget);

    startTransition(async () => {
      const result = await updateProfileAction(formData);
      if ('success' in result && result.success) {
        setSuccess('Profile updated successfully!');
      } else {
        setError((result as { error: string }).error || 'Failed to update profile');
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

      <div className="row">
        <div className="col-md-6 mb-3">
          <label htmlFor="firstName" className="form-label fw-semibold">
            First Name
          </label>
          <input
            id="firstName"
            name="firstName"
            type="text"
            className="form-control"
            defaultValue={user.firstName || ''}
            required
          />
        </div>
        <div className="col-md-6 mb-3">
          <label htmlFor="lastName" className="form-label fw-semibold">
            Last Name
          </label>
          <input
            id="lastName"
            name="lastName"
            type="text"
            className="form-control"
            defaultValue={user.lastName || ''}
            required
          />
        </div>
      </div>

      <div className="mb-3">
        <label htmlFor="userName" className="form-label fw-semibold">
          Username
        </label>
        <input
          id="userName"
          name="userName"
          type="text"
          className="form-control"
          defaultValue={user.userName || ''}
          required
        />
      </div>

      <div className="mb-3">
        <label className="form-label fw-semibold">Email Address</label>
        <input
          type="email"
          className="form-control"
          value={user.email}
          disabled
          readOnly
        />
        <div className="form-text">Email cannot be changed here.</div>
      </div>

      <div className="d-flex gap-2">
        <button type="submit" className="btn btn-primary" disabled={isPending}>
          {isPending ? (
            <>
              <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
              Saving...
            </>
          ) : (
            <>
              <i className="bi bi-check-lg me-1"></i>
              Save Changes
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
