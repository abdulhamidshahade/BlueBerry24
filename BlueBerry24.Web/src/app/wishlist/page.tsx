import { redirect } from 'next/navigation';

/** Canonical wishlist URL is /profile/wishlist; this keeps /wishlist links working. */
export default function WishlistAliasPage() {
  redirect('/profile/wishlist');
}
