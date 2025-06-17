import { cookies } from 'next/headers';

export interface ApiRequestOptions extends RequestInit {
  requireAuth?: boolean;
  isPublic?: boolean;
}

export async function apiRequest<T>(
  url: string, 
  options: ApiRequestOptions = {}
): Promise<T> {
  const { requireAuth = false, isPublic = false, headers = {}, ...fetchOptions } = options;
  
  let authHeaders = {};
  if (requireAuth && !isPublic) {
    const cookieStore = await cookies();
    const token = cookieStore.get('auth_token')?.value;
    
    if (token) {
      authHeaders = {
        'Authorization': `Bearer ${token}`
      };
    }
  }

  const defaultHeaders = {
    'Content-Type': 'application/json',
    ...authHeaders,
    ...headers,
  };

  try {
    const response = await fetch(url, {
      ...fetchOptions,
      headers: defaultHeaders,
    });

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
      try {
        const errorText = await response.text();
        if (errorText) {
          try {
            const errorJson = JSON.parse(errorText);
            errorDetails = errorJson.message || errorJson.statusMessage || errorJson.title || errorText;
          } catch {
            errorDetails = errorText;
          }
        }
      } catch {
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

    const contentType = response.headers.get('content-type');
    if (!contentType || !contentType.includes('application/json')) {
      throw new Error('Response is not JSON');
    }

    const text = await response.text();
    if (!text) {
      throw new Error('Empty response body');
    }

    return JSON.parse(text) as T;
  } catch (error) {
    console.error('API request failed:', error);
    throw error;
  }
}