import { NextRequest, NextResponse } from 'next/server';
import { cookies } from 'next/headers';

export async function POST(request: NextRequest) {
  try {
    const cookieStore = await cookies();
    
    cookieStore.delete('auth_token');
    cookieStore.delete('user_info');

    return NextResponse.redirect(new URL('/', request.url));
  } catch (error) {
    console.error('Logout error:', error);
    return NextResponse.redirect(new URL('/', request.url));
  }
} 