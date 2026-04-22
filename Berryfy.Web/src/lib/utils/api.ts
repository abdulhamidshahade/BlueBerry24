import { cookies } from 'next/headers';

export interface ApiRequestOptions extends RequestInit {
  requireAuth?: boolean;
  isPublic?: boolean;
}

async function parseJsonResponse<T>(response: Response): Promise<T> {
  const contentType = response.headers.get('content-type');
  if (!contentType || !contentType.includes('application/json')) {
    throw new Error('Response is not JSON');
  }
  const text = await response.text();
  if (!text) {
    throw new Error('Empty response body');
  }
  return JSON.parse(text) as T;
}

async function attemptTokenRefresh(): Promise<string | null> {
  const cookieStore = await cookies();
  const refreshToken = cookieStore.get('refresh_token')?.value;
  if (!refreshToken) return null;

  try {
    const response = await fetch(`${process.env.API_BASE_AUTH}/refresh-token`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ token: refreshToken }),
    });

    if (!response.ok) return null;

    const result = await response.json();
    if (!result.isSuccess || !result.data?.token) return null;

    const { isSecure } = await import('../cookie-is-secure-server').then(m => ({ isSecure: m.cookieIsSecure() }));
    const secure = await isSecure;

    cookieStore.set('auth_token', result.data.token, {
      path: '/',
      httpOnly: true,
      secure,
      sameSite: 'lax',
      maxAge: 2 * 60 * 60,
    });

    if (result.data.refreshToken) {
      cookieStore.set('refresh_token', result.data.refreshToken, {
        path: '/',
        httpOnly: true,
        secure,
        sameSite: 'lax',
        maxAge: 7 * 24 * 60 * 60,
      });
    }

    if (result.data.user) {
      cookieStore.set('user_info', JSON.stringify(result.data.user), {
        path: '/',
        httpOnly: true,
        secure,
        sameSite: 'lax',
        maxAge: 7 * 24 * 60 * 60,
      });
    }

    return result.data.token as string;
  } catch {
    return null;
  }
}

export async function apiRequest<T>(
  url: string, 
  options: ApiRequestOptions = {}
): Promise<T> {
  const { requireAuth = false, isPublic = false, headers = {}, ...fetchOptions } = options;
  
  const cookieStore = await cookies();

  const buildHeaders = (token?: string): Record<string, string> => {
    const h: Record<string, string> = {
      'Content-Type': 'application/json',
      ...(headers as Record<string, string>),
    };
    if (token) h['Authorization'] = `Bearer ${token}`;
    return h;
  };

  const token = (requireAuth && !isPublic)
    ? cookieStore.get('auth_token')?.value
    : undefined;

  try {
    const response = await fetch(url, {
      ...fetchOptions,
      headers: buildHeaders(token),
      credentials: 'include',
    });

    // --- 401 handling: attempt refresh then retry once ---
    if (response.status === 401 && requireAuth && !isPublic) {
      const newToken = await attemptTokenRefresh();
      if (newToken) {
        const retryResponse = await fetch(url, {
          ...fetchOptions,
          headers: buildHeaders(newToken),
          credentials: 'include',
        });

        if (retryResponse.ok) {
          return parseJsonResponse<T>(retryResponse);
        }
      }
      // Refresh failed or retry still 401 — fall through to throw
    }

    if (!response.ok) {
      if (response.status === 401 && isPublic) {
        console.warn(`Authentication required for public endpoint: ${url}`);
        return {
          isSuccess: false,
          data: null,
          statusMessage: 'Authentication required'
        } as T;
      }
      
      let errorDetails = '';
      let parsedError: any = null;
      try {
        const errorText = await response.text();
        if (errorText) {
          try {
            parsedError = JSON.parse(errorText);
            errorDetails = parsedError.message || parsedError.statusMessage || parsedError.title || errorText;
          } catch {
            errorDetails = errorText;
          }
        }
      } catch {
      }
      
      // Handle 404 gracefully for GET requests
      if (response.status === 404 && (!options.method || options.method === 'GET')) {
        if (parsedError) {
          return parsedError as T;
        }
        return {
          isSuccess: false,
          statusCode: 404,
          statusMessage: errorDetails || 'Not found',
          data: null
        } as T;
      }
      
      const errorMessage = `HTTP error! status: ${response.status}${errorDetails ? ` - ${errorDetails}` : ''}`;
      console.error('API Request Error:', {
        url,
        status: response.status,
        statusText: response.statusText,
        errorDetails,
        requestOptions: fetchOptions
      });
      
      throw new Error(errorMessage);
    }

    return parseJsonResponse<T>(response);
  } catch (error) {
    console.error('API request failed:', error);
    throw error;
  }
}