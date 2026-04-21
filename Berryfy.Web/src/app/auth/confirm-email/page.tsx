import OtpConfirmationForm from '../../../components/auth/OtpConfirmationForm';
import { getCurrentUser } from '../../../lib/actions/auth-actions';
import { redirect } from 'next/navigation';

interface EmailConfirmationPageProps {
  searchParams: Promise<{
    email?: string;
    error?: string;
  }>;
}

export default async function EmailConfirmationPage({ searchParams }: EmailConfirmationPageProps) {
  const user = await getCurrentUser();
  if (user) {
    redirect('/');
  }

  const params = await searchParams;
  const { email, error } = params;

  if (!email) {
    redirect('/auth/resend-confirmation?error=Missing email address. Please request a new confirmation code.');
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
          <OtpConfirmationForm email={email} />
        </div>
      </div>
    </div>
  );
}

export const metadata = {
  title: 'Verify Email - Berryfy',
  description: 'Enter your 6-digit verification code to activate your Berryfy account',
};
