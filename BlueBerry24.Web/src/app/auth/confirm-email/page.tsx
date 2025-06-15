import EmailConfirmationForm from '@/components/auth/EmailConfirmationForm';
import { getCurrentUser } from '@/lib/actions/auth-actions';
import { redirect } from 'next/navigation';

interface EmailConfirmationPageProps {
  searchParams: Promise<{
    email?: string;
    token?: string;
    error?: string;
  }>;
}

export default async function EmailConfirmationPage({ searchParams }: EmailConfirmationPageProps) {
  const user = await getCurrentUser();
  if (user) {
    redirect('/');
  }

  const params = await searchParams;
  const { email, token, error } = params;

  if (!email || !token) {
    redirect('/auth/resend-confirmation?error=Invalid or missing confirmation link. Please request a new confirmation email.');
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

          <EmailConfirmationForm email={email} token={token} />
        </div>
      </div>
    </div>
  );
}

export const metadata = {
  title: 'Confirm Email - BlueBerry24',
  description: 'Confirm your BlueBerry24 account email address',
}; 