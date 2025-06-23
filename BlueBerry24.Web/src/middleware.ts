import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

export function middleware(request: NextRequest) {
  const response = NextResponse.next();

  const session = request.cookies.get("cart_session");
  if (!session) {
    const newSessionId = crypto.randomUUID();
    response.cookies.set("cart_session", newSessionId, {
      path: "/",
      httpOnly: true,
      sameSite: "lax",
      secure: true,
      maxAge: 30 * 24 * 60 * 60,
    });
  }

  const authToken = request.cookies.get("auth_token");
  const { pathname } = request.nextUrl;

  const protectedRoutes = ["/profile", "/orders", "/admin"];
  const isProtectedRoute = protectedRoutes.some((route) =>
    pathname.startsWith(route)
  );

  if (authToken) {
    response.cookies.set("cart_session", "", {
      path: "/",
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
