import { resendConfirmationAction } from '@/lib/actions/auth-actions';
import { redirect } from 'next/navigation';

interface ResendConfirmationFormProps {
  email?: string;
}

export default function ResendConfirmationForm({ email }: ResendConfirmationFormProps) {
  async function handleResendConfirmation(formData: FormData) {
    'use server';
    
    const result = await resendConfirmationAction(formData);
    
    if (result.success) {
      redirect('/auth/resend-confirmation?message=If your email exists in our system, a new confirmation link has been sent.');
    } else {
      redirect(`/auth/resend-confirmation?error=${encodeURIComponent(result.error || 'Failed to send confirmation email')}`);
    }
  }

  return (
    <div className="card shadow">
      <div className="card-body p-4">
        <div className="text-center mb-4">
          <h2 className="card-title">
            <i className="bi bi-envelope-arrow-up me-2"></i>
            Resend Confirmation Email
          </h2>
          <p className="text-muted">
            Enter your email address and we'll send you a new confirmation link.
          </p>
        </div>

        <form action={handleResendConfirmation}>
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
              defaultValue={email}
              required
            />
          </div>

          <button type="submit" className="btn btn-primary w-100 mb-3">
            <i className="bi bi-send me-2"></i>
            Send Confirmation Email
          </button>

          <div className="text-center">
            <p className="mb-0">
              Already confirmed your email?{' '}
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