import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";
import { cookieIsSecureForRequest } from "@/lib/cookie-is-secure-request";

async function tryRefreshToken(
  refreshTokenValue: string,
  secure: boolean,
  response: NextResponse
): Promise<string | null> {
  try {
    const apiBase = process.env.API_BASE_AUTH;
    const res = await fetch(`${apiBase}/refresh-token`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ token: refreshTokenValue }),
    });

    if (!res.ok) return null;

    const result = await res.json();
    if (!result.isSuccess || !result.data?.token) return null;

    const { token, refreshToken, user } = result.data;

    response.cookies.set("auth_token", token, {
      path: "/",
      httpOnly: true,
      secure,
      sameSite: "lax",
      maxAge: 2 * 60 * 60,
    });

    if (refreshToken) {
      response.cookies.set("refresh_token", refreshToken, {
        path: "/",
        httpOnly: true,
        secure,
        sameSite: "lax",
        maxAge: 7 * 24 * 60 * 60,
      });
    }

    if (user) {
      response.cookies.set("user_info", JSON.stringify(user), {
        path: "/",
        httpOnly: true,
        secure,
        sameSite: "lax",
        maxAge: 7 * 24 * 60 * 60,
      });
    }

    return token as string;
  } catch {
    return null;
  }
}

export async function middleware(request: NextRequest) {
  const response = NextResponse.next();
  const secure = cookieIsSecureForRequest(request);

  const session = request.cookies.get("cart_session");
  if (!session) {
    const newSessionId = crypto.randomUUID();
    response.cookies.set("cart_session", newSessionId, {
      path: "/",
      httpOnly: true,
      sameSite: "lax",
      secure,
      maxAge: 30 * 24 * 60 * 60,
    });
  }

  let authToken = request.cookies.get("auth_token");
  const refreshTokenCookie = request.cookies.get("refresh_token");
  const { pathname } = request.nextUrl;

  // If access token is missing but refresh token exists, silently refresh
  if (!authToken && refreshTokenCookie) {
    const newToken = await tryRefreshToken(refreshTokenCookie.value, secure, response);
    if (newToken) {
      authToken = { name: "auth_token", value: newToken };
    }
  }

  const protectedRoutes = ["/profile", "/orders", "/wishlist", "/admin", "/checkout"];
  const isProtectedRoute = protectedRoutes.some((route) =>
    pathname.startsWith(route)
  );

  if (authToken) {
    response.cookies.set("cart_session", "", {
      path: "/",
      httpOnly: true,
      sameSite: "lax",
      secure,
      maxAge: 0,
    });
  }

  if (isProtectedRoute && !authToken) {
    const loginUrl = new URL("/auth/login", request.url);
    loginUrl.searchParams.set("redirectTo", pathname);
    return NextResponse.redirect(loginUrl);
  }

  const authPages = ["/auth/login", "/auth/register"];
  const isAuthPage = authPages.some((page) => pathname.startsWith(page));

  if (isAuthPage && authToken) {
    return NextResponse.redirect(new URL("/", request.url));
  }

  return response;
}

export const config = {
  matcher: ["/((?!api|_next/static|favicon.ico).*)"],
};
