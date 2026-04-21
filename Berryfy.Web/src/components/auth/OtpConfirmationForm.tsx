'use client';

import { useRef, useState, useTransition, KeyboardEvent, ClipboardEvent } from 'react';
import { confirmEmailAction, resendConfirmationAction } from '../../lib/actions/auth-actions';

interface OtpConfirmationFormProps {
  email: string;
}

const CODE_LENGTH = 6;

export default function OtpConfirmationForm({ email }: OtpConfirmationFormProps) {
  const [digits, setDigits] = useState<string[]>(Array(CODE_LENGTH).fill(''));
  const [error, setError] = useState('');
  const [success, setSuccess] = useState(false);
  const [resendMessage, setResendMessage] = useState('');
  const [isPending, startTransition] = useTransition();
  const [isResending, startResendTransition] = useTransition();
  const inputRefs = useRef<(HTMLInputElement | null)[]>([]);

  const code = digits.join('');
  const isComplete = code.length === CODE_LENGTH && digits.every((d) => d !== '');

  function handleChange(index: number, value: string) {
    const cleaned = value.replace(/\D/g, '').slice(-1);
    const next = [...digits];
    next[index] = cleaned;
    setDigits(next);
    setError('');

    if (cleaned && index < CODE_LENGTH - 1) {
      inputRefs.current[index + 1]?.focus();
    }
  }

  function handleKeyDown(index: number, e: KeyboardEvent<HTMLInputElement>) {
    if (e.key === 'Backspace') {
      if (digits[index]) {
        const next = [...digits];
        next[index] = '';
        setDigits(next);
      } else if (index > 0) {
        inputRefs.current[index - 1]?.focus();
      }
    } else if (e.key === 'ArrowLeft' && index > 0) {
      inputRefs.current[index - 1]?.focus();
    } else if (e.key === 'ArrowRight' && index < CODE_LENGTH - 1) {
      inputRefs.current[index + 1]?.focus();
    }
  }

  function handlePaste(e: ClipboardEvent<HTMLInputElement>) {
    e.preventDefault();
    const pasted = e.clipboardData.getData('text').replace(/\D/g, '').slice(0, CODE_LENGTH);
    if (!pasted) return;
    const next = Array(CODE_LENGTH).fill('');
    for (let i = 0; i < pasted.length; i++) next[i] = pasted[i];
    setDigits(next);
    setError('');
    const focusIdx = Math.min(pasted.length, CODE_LENGTH - 1);
    inputRefs.current[focusIdx]?.focus();
  }

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (!isComplete) {
      setError('Please enter all 6 digits.');
      return;
    }
    setError('');

    const formData = new FormData();
    formData.set('email', email);
    formData.set('code', code);

    startTransition(async () => {
      const result = await confirmEmailAction(formData);
      if (result.success) {
        setSuccess(true);
        setTimeout(() => {
          window.location.href = '/auth/login?message=' +
            encodeURIComponent('Email verified! You can now sign in.');
        }, 1500);
      } else {
        setError(result.error || 'Invalid or expired code. Please try again.');
        setDigits(Array(CODE_LENGTH).fill(''));
        inputRefs.current[0]?.focus();
      }
    });
  }

  function handleResend() {
    setError('');
    setResendMessage('');
    const formData = new FormData();
    formData.set('email', email);

    startResendTransition(async () => {
      const result = await resendConfirmationAction(formData);
      if (result.success) {
        setResendMessage('A new code has been sent to your email.');
        setDigits(Array(CODE_LENGTH).fill(''));
        inputRefs.current[0]?.focus();
      } else {
        setError(result.error || 'Failed to resend code.');
      }
    });
  }

  return (
    <div className="card shadow">
      <div className="card-body p-4">
        <div className="text-center mb-4">
          <div className="mb-3">
            <i className="bi bi-envelope-check text-primary" style={{ fontSize: '3rem' }}></i>
          </div>
          <h2 className="card-title fw-bold">Check your email</h2>
          <p className="text-muted mb-1">We sent a 6-digit code to</p>
          <p className="fw-semibold">{email}</p>
        </div>

        {error && (
          <div className="alert alert-danger py-2" role="alert">
            <i className="bi bi-exclamation-triangle me-2"></i>
            {error}
          </div>
        )}

        {resendMessage && (
          <div className="alert alert-success py-2" role="alert">
            <i className="bi bi-check-circle me-2"></i>
            {resendMessage}
          </div>
        )}

        {success && (
          <div className="alert alert-success py-2" role="alert">
            <i className="bi bi-check-circle-fill me-2"></i>
            Email verified! Redirecting…
          </div>
        )}

        <form onSubmit={handleSubmit}>
          <div className="d-flex justify-content-center gap-2 mb-4">
            {digits.map((digit, i) => (
              <input
                key={i}
                ref={(el) => { inputRefs.current[i] = el; }}
                type="text"
                inputMode="numeric"
                maxLength={1}
                value={digit}
                onChange={(e) => handleChange(i, e.target.value)}
                onKeyDown={(e) => handleKeyDown(i, e)}
                onPaste={handlePaste}
                className="form-control text-center fw-bold fs-4"
                style={{
                  width: '48px',
                  height: '56px',
                  borderRadius: '8px',
                  borderColor: digit ? '#0d6efd' : undefined,
                  caretColor: 'transparent',
                }}
                aria-label={`Digit ${i + 1}`}
                disabled={isPending || success}
                autoFocus={i === 0}
              />
            ))}
          </div>

          <button
            type="submit"
            className="btn btn-primary w-100 mb-3"
            disabled={!isComplete || isPending || success}
          >
            {isPending ? (
              <>
                <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true" />
                Verifying…
              </>
            ) : (
              <>
                <i className="bi bi-check-circle me-2"></i>
                Verify Email
              </>
            )}
          </button>
        </form>

        <div className="text-center">
          <p className="text-muted mb-1 small">Didn&apos;t receive the code?</p>
          <button
            className="btn btn-link p-0 small"
            onClick={handleResend}
            disabled={isResending || isPending}
          >
            {isResending ? 'Sending…' : 'Resend code'}
          </button>
        </div>

        <div className="text-center mt-3">
          <a href="/auth/resend-confirmation" className="text-decoration-none small text-muted">
            Use a different email address
          </a>
        </div>
      </div>
    </div>
  );
}
