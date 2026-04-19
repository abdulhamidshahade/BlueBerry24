import type { NextRequest } from 'next/server';
import { cookieSecureFromEnv, forwardedProtoIsHttps } from './cookie-is-secure-shared';

export function cookieIsSecureForRequest(request: NextRequest): boolean {
  const fromForwarded = forwardedProtoIsHttps(request.headers.get('x-forwarded-proto'));
  if (fromForwarded !== undefined) return fromForwarded;
  if (request.nextUrl.protocol === 'https:') return true;
  return cookieSecureFromEnv();
}
