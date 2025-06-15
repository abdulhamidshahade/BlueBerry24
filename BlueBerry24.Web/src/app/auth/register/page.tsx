import RegisterForm from '@/components/auth/RegisterForm';
import { getCurrentUser } from '@/lib/actions/auth-actions';
import { redirect } from 'next/navigation';

interface RegisterPageProps {
  searchParams: Promise<{
    error?: string;
  }>;
}

export default async function RegisterPage({ searchParams }: RegisterPageProps) {
  const user = await getCurrentUser();
  if (user) {
    redirect('/');
  }

  const params = await searchParams;
  const { error } = params;

  return (
    <div className="container py-5">
      <div className="row justify-content-center">
        <div className="col-md-8 col-lg-6">
          {error && (
            <div className="alert alert-danger mb-4" role="alert">
              <i className="bi bi-exclamation-triangle me-2"></i>
              {decodeURIComponent(error)}
            </div>
          )}

          <RegisterForm />
        </div>
      </div>
    </div>
  );
}

export const metadata = {
  title: 'Create Account - BlueBerry24',
  description: 'Create your BlueBerry24 account and start shopping',
}; 