import { headers } from 'next/headers';
import { cookieSecureFromEnv, forwardedProtoIsHttps } from './cookie-is-secure-shared';

export async function cookieIsSecure(): Promise<boolean> {
  const h = await headers();
  const fromForwarded = forwardedProtoIsHttps(h.get('x-forwarded-proto'));
  if (fromForwarded !== undefined) return fromForwarded;
  return cookieSecureFromEnv();
}
