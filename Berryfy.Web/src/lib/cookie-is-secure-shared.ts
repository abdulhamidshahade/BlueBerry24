export function forwardedProtoIsHttps(value: string | null): boolean | undefined {
  if (value == null || value === '') return undefined;
  return value.split(',')[0].trim().toLowerCase() === 'https';
}

export function cookieSecureFromEnv(): boolean {
  if (process.env.NODE_ENV !== 'production') return false;
  if (process.env.COOKIE_SECURE === 'false') return false;
  return true;
}
