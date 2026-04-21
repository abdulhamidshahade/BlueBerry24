import { getCurrentUser } from '../../../lib/actions/auth-actions';
import { redirect } from 'next/navigation';
import EditProfileForm from '../../../components/auth/EditProfileForm';

export const metadata = {
  title: 'Edit Profile - Berryfy',
  description: 'Update your Berryfy account profile',
};

export default async function EditProfilePage() {
  const user = await getCurrentUser();

  if (!user) {
    redirect('/auth/login?redirectTo=/profile/edit');
  }

  return (
    <div className="container py-5">
      <div className="row">
        <div className="col-md-7 mx-auto">
          <div className="card shadow">
            <div className="card-header bg-primary text-white">
              <h2 className="card-title mb-0">
                <i className="bi bi-pencil-square me-2"></i>
                Edit Profile
              </h2>
            </div>
            <div className="card-body p-4">
              <EditProfileForm user={user} />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
