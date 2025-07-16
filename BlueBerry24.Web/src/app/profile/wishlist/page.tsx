import { redirect } from 'next/navigation';
import { getCurrentUser } from '../../../lib/actions/auth-actions';
import { getUserWishlists } from '../../../lib/actions/wishlist-actions';
import WishlistManagement from '../../../components/layout/WishlistManagement';

export const dynamic = 'force-dynamic';

interface WishlistPageProps {
  searchParams: Promise<{
    modal?: string;
    id?: string;
    productId?: string;
    success?: string;
    error?: string;
  }>;
}

export default async function WishlistPage({ searchParams }: WishlistPageProps) {
  const user = await getCurrentUser();
  const resolvedSearchParams = await searchParams;
  if (!user) {
    redirect('/auth/login?redirectTo=/profile/wishlist');
  }

  let wishlists;
  try {
    wishlists = await getUserWishlists();
  } catch (error) {
    console.error('Failed to fetch wishlists:', error);
    redirect('/profile/wishlist?error=' + encodeURIComponent('Failed to load wishlists'));
  }

  return (
    <div className="container-fluid py-4">
      <div className="row">
        <div className="col-12">
          <WishlistManagement 
            wishlists={wishlists}
            currentUser={user}
            showModal={resolvedSearchParams.modal}
            selectedWishlistId={resolvedSearchParams.id}
            selectedProductId={resolvedSearchParams.productId}
            success={resolvedSearchParams.success}
            error={resolvedSearchParams.error}
          />
        </div>
      </div>
    </div>
  );
}

export const metadata = {
  title: 'My Wishlists | BlueBerry24',
  description: 'Manage your wishlists and saved products on BlueBerry24.',
}; 