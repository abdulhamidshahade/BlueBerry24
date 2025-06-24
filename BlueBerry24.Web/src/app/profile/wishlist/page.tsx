import { redirect } from 'next/navigation';
import { getCurrentUser } from '@/lib/actions/auth-actions';
import { getUserWishlists } from '@/lib/actions/wishlist-actions';
import WishlistManagement from '@/components/layout/WishlistManagement';

interface WishlistPageProps {
  searchParams: {
    modal?: string;
    id?: string;
    productId?: string;
    success?: string;
    error?: string;
  };
}

export default async function WishlistPage({ searchParams }: WishlistPageProps) {
  const user = await getCurrentUser();
  
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
            showModal={searchParams.modal}
            selectedWishlistId={searchParams.id}
            selectedProductId={searchParams.productId}
            success={searchParams.success}
            error={searchParams.error}
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