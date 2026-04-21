import { getCurrentUser } from '../../../lib/actions/auth-actions';
import { redirect } from 'next/navigation';
import ChangePasswordForm from '../../../components/auth/ChangePasswordForm';

export const metadata = {
  title: 'Change Password - Berryfy',
  description: 'Update your Berryfy account password',
};

export default async function ChangePasswordPage() {
  const user = await getCurrentUser();

  if (!user) {
    redirect('/auth/login?redirectTo=/profile/change-password');
  }

  return (
    <div className="container py-5">
      <div className="row">
        <div className="col-md-6 mx-auto">
          <div className="card shadow">
            <div className="card-header bg-primary text-white">
              <h2 className="card-title mb-0">
                <i className="bi bi-key me-2"></i>
                Change Password
              </h2>
            </div>
            <div className="card-body p-4">
              <ChangePasswordForm />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
