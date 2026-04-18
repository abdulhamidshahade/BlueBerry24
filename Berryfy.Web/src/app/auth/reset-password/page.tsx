import ResetPasswordForm from '../../../components/auth/ResetPasswordForm';
import { getCurrentUser } from '../../../lib/actions/auth-actions';
import { redirect } from 'next/navigation';

interface ResetPasswordPageProps {
  searchParams: Promise<{
    email?: string;
    token?: string;
    error?: string;
  }>;
}

export default async function ResetPasswordPage({ searchParams }: ResetPasswordPageProps) {
  const user = await getCurrentUser();
  if (user) {
    redirect('/');
  }

  const params = await searchParams;
  const { email, token, error } = params;

  if (!email || !token) {
    redirect('/auth/forgot-password?error=Invalid or missing reset link. Please request a new password reset.');
  }

  return (
    <div className="container py-5">
      <div className="row justify-content-center">
        <div className="col-md-6 col-lg-5">
          {error && (
            <div className="alert alert-danger mb-4" role="alert">
              <i className="bi bi-exclamation-triangle me-2"></i>
              {decodeURIComponent(error)}
            </div>
          )}

          <ResetPasswordForm email={email} token={token} />
        </div>
      </div>
    </div>
  );
}

export const metadata = {
  title: 'Reset Password - BlueBerry24',
  description: 'Reset your BlueBerry24 account password',
}; 