import { redirect } from 'next/navigation';
import { getCurrentUser } from '@/lib/actions/auth-actions';
import { getAllWishlists, getGlobalWishlistStats } from '@/lib/actions/admin-wishlist-actions';
import AdminWishlistManagement from '@/components/admin/AdminWishlistManagement';

interface AdminWishlistPageProps {
  searchParams: {
    modal?: string;
    id?: string;
    filter?: string;
    success?: string;
    error?: string;
  };
}

export default async function AdminWishlistPage({ searchParams }: AdminWishlistPageProps) {
  const user = await getCurrentUser();
  
  if (!user) {
    redirect('/auth/login?redirectTo=/admin/wishlists');
  }

  // TODO: Add admin role check here!
  // if (!user.roles?.includes('Admin')) {
  //   redirect('/dashboard?error=Unauthorized');
  // }

  let wishlists;
  let stats;
  
  try {
    [wishlists, stats] = await Promise.all([
      getAllWishlists(),
      getGlobalWishlistStats()
    ]);
  } catch (error) {
    console.error('Failed to fetch admin wishlist data:', error);
    redirect('/admin/wishlists?error=' + encodeURIComponent('Failed to load wishlist data'));
  }

  return (
    <div className="container-fluid py-4">
      <div className="row">
        <div className="col-12">
          <AdminWishlistManagement 
            wishlists={wishlists}
            stats={stats}
            currentUser={user}
            showModal={searchParams.modal}
            selectedWishlistId={searchParams.id}
            success={searchParams.success}
            error={searchParams.error}
          />
        </div>
      </div>
    </div>
  );
}

export const metadata = {
  title: 'Wishlist Management | Admin Dashboard | BlueBerry24',
  description: 'Manage and monitor all user wishlists on BlueBerry24.',
}; 