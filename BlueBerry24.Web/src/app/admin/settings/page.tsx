import { redirect } from 'next/navigation';
import { getCurrentUser } from '@/lib/actions/auth-actions';
import { getSystemSettings } from '@/lib/actions/settings-action'
import SettingsManagement from '@/components/admin/SettingsManagement';

interface SettingsPageProps {
  searchParams: {
    modal?: string;
    success?: string;
    error?: string;
  };
}

export default async function SettingsPage({ searchParams }: SettingsPageProps) {
  const user = await getCurrentUser();
  
  if (!user) {
    redirect('/auth/login?redirectTo=/admin/settings');
  }

  const userRoles = user.roles || [];
  const hasAdminRole = userRoles.some(role => 
    role.toLowerCase().includes('admin') || 
    role.toLowerCase().includes('superadmin')
  );

  if (!hasAdminRole) {
    redirect('/?error=' + encodeURIComponent('You do not have permission to access this page'));
  }

  const settings = await getSystemSettings();

  return (
    <div className="p-4">
      <div className="row">
        <div className="col-12">
          <SettingsManagement 
            settings={settings}
            currentUser={user}
            showModal={searchParams.modal}
            success={searchParams.success}
            error={searchParams.error}
          />
        </div>
      </div>
    </div>
  );
}

export const metadata = {
  title: 'Settings | Admin Dashboard | BlueBerry24',
  description: 'Manage system settings and configuration for BlueBerry24 e-commerce platform.',
};