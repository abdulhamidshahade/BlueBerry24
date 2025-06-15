import LoginForm from '@/components/auth/LoginForm';
import { getCurrentUser } from '@/lib/actions/auth-actions';
import { redirect } from 'next/navigation';

interface LoginPageProps {
  searchParams: Promise<{
    error?: string;
    message?: string;
    redirectTo?: string;
  }>;
}

export default async function LoginPage({ searchParams }: LoginPageProps) {
  const user = await getCurrentUser();
  if (user) {
    redirect('/');
  }

  const params = await searchParams;
  const { error, message, redirectTo } = params;

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
          
          {message && (
            <div className="alert alert-success mb-4" role="alert">
              <i className="bi bi-check-circle me-2"></i>
              {decodeURIComponent(message)}
            </div>
          )}

          <LoginForm redirectTo={redirectTo} />
        </div>
      </div>
    </div>
  );
}

export const metadata = {
  title: 'Sign In - BlueBerry24',
  description: 'Sign in to your BlueBerry24 account',
}; 