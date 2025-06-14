import {
  CartDto,
  AddToCartDto,
  UpdateCartItemDto,
  ApplyCouponDto,
  CheckoutRequest,
  CheckoutResponse,
} from "@/types/cart";
import { ICartService } from "./interface";
import { ResponseDto } from "@/types/responseDto";
import { apiRequest } from "@/lib/utils/api";
import { cookies } from "next/headers";

process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
const API_BASE = process.env.API_BASE_SHOPPING_CART;

export class CartService implements ICartService {
  async getByUserId(): Promise<CartDto | null> {
    console.log("CartService.getBySessionId called with sessionId:");

    const url = `${API_BASE}/user-id`;
    console.log("Making API call to:", url);

    try {
      const json: ResponseDto<CartDto> = await apiRequest(url, {
        cache: "no-store",
        requireAuth: true,
      });

      console.log("Cart API response:", json);

      if (!json.isSuccess || !json.data) {
        console.error("Cart API returned unsuccessful response:", json);

        if (json.statusCode === 404) {
          return null;
        }
        return json.data;
      }

      console.log("Retrieved cart:", json.data);
      return json.data;
    } catch (error) {
      console.error("Failed to fetch cart by id:", error);
      throw new Error("Failed to fetch cart");
    }
  }
  async getById(id: number): Promise<CartDto> {
    try {
      const json: ResponseDto<CartDto> = await apiRequest(`${API_BASE}/${id}`, {
        cache: "no-store",
      });

      if (!json.isSuccess || !json.data) {
        throw new Error(json.statusMessage || "Failed to fetch cart");
      }
      return json.data;
    } catch (error) {
      console.error("Failed to fetch cart by id:", error);
      throw new Error("Failed to fetch cart");
    }
  }

  async getBySessionId(sessionId: string): Promise<CartDto | null> {
    console.log("CartService.getBySessionId called with sessionId:", sessionId);

    const url = `${API_BASE}/session-id`;
    console.log("Making API call to:", url);

    try {
      const json: ResponseDto<CartDto> = await apiRequest(url, {
        cache: "no-store",
        headers: {
          "X-Session-Id": sessionId ?? "",
        },
      });

      console.log("Cart API response:", json);

      if (!json) {
        return null;
      }

      if (!json.isSuccess || !json.data) {
        console.error("Cart API returned unsuccessful response:", json);
        throw new Error(json.statusMessage || "Failed to fetch cart");
      }

      console.log("Retrieved cart:", json.data);
      return json.data;
    } catch (error) {
      console.error("Failed to fetch cart by session:", error);
      throw new Error("Failed to fetch cart");
    }
  }

  async create(): Promise<CartDto> {
    const cookieStore = await cookies();
    const token = cookieStore.get("cart_session")?.value;

    const options: any = {
      method: "POST",
    };

    if (token) {
      options.headers = {
        "X-Session-Id": token ?? "",
      };
    } else {
      options.requireAuth = true;
    }

    try {
      const json: ResponseDto<CartDto> = await apiRequest(
        `${API_BASE}/create`,

        options
      );

      if (!json.isSuccess || !json.data) {
        throw new Error(json.statusMessage || "Failed to create cart");
      }
      return json.data;
    } catch (error) {
      console.error("Failed to create cart:", error);
      throw new Error("Failed to create cart");
    }
  }

  async addItem(data: AddToCartDto): Promise<CartDto> {
    const cookieStore = await cookies();
    const token = cookieStore.get("cart_session")?.value;

    const options: any = {
      method: "POST",
      body: JSON.stringify(data),
    };

    if (token) {
      options.headers = {
        "X-Session-Id": token ?? "",
      };
    } else {
      options.requireAuth = true;
    }

    try {
      const json: ResponseDto<CartDto> = await apiRequest(
        `${API_BASE}/add-item`,

        options
      );

      if (!json.isSuccess || !json.data) {
        throw new Error(json.statusMessage || "Failed to add item to cart");
      }
      return json.data;
    } catch (error) {
      console.error("Failed to add item to cart:", error);
      throw new Error("Failed to add item to cart");
    }
  }

  async updateItemQuantity(
    cartId: number,
    data: UpdateCartItemDto
  ): Promise<CartDto> {
    const requestBody = {
      cartId: cartId,
      productId: data.productId,
      quantity: data.quantity,
      userId: data.userId,
      sessionId: data.sessionId,
    };

    var cookie = await cookies();

    var token = cookie.get("cart_session")?.value;

    const options: any = {
      method: "PUT",
      body: JSON.stringify(requestBody),
    };

    if (token) {
      options.headers = {
        "X-Session-Id": token ?? "",
      };
    } else {
      options.requireAuth = true;
    }

    try {
      const json: ResponseDto<CartDto> = await apiRequest(
        `${API_BASE}/${cartId}/update`,
        options
      );

      if (!json.isSuccess || !json.data) {
        throw new Error(json.statusMessage || "Failed to update item quantity");
      }
      return json.data;
    } catch (error) {
      console.error("Failed to update item quantity:", error);
      throw new Error("Failed to update item quantity");
    }
  }

  async removeItem(cartId: number, productId: number): Promise<boolean> {
    const cookie = await cookies();

    var token = await cookie.get("cart_session")?.value;

    const options: any = {
      method: "DELETE",
    };

    if (token) {
      options.headers = {
        "X-Session-Id": token ?? "",
      };
    } else {
      options.requireAuth = true;
    }

    try {
      const response: ResponseDto<boolean> = await apiRequest(
        `${API_BASE}/${cartId}/remove/${productId}`,

        options
      );

      if (!response.isSuccess) {
        throw new Error(
          `Failed to remove item from cart: ${response.statusMessage}`
        );
      }

      return response.statusCode === 204;
    } catch (error) {
      console.error("Failed to remove item from cart:", error);
      throw new Error("Failed to remove item from cart");
    }
  }

  async clearCart(cartId: number): Promise<boolean> {
    const cookieStore = await cookies();
    const token = cookieStore.get("cart_session")?.value;

    const options: any = {
      method: "DELETE",
    };

    if (token) {
      options.headers = {
        "X-Session-Id": token ?? "",
      };
    } else {
      options.requireAuth = true;
    }

    try {
      const response: ResponseDto<boolean> = await apiRequest(
        `${API_BASE}/${cartId}/clear`,
        options
      );

      if (!response.isSuccess) {
        throw new Error(`Failed to clear cart: ${response.statusMessage}`);
      }

      return response.statusCode === 204;
    } catch (error) {
      console.error("Failed to clear cart:", error);
      throw new Error("Failed to clear cart");
    }
  }

  async completeCart(cartId: number): Promise<boolean> {
    try {
      const json: ResponseDto<boolean> = await apiRequest(
        `${API_BASE}/${cartId}/complete`,
        {
          method: "POST",
          requireAuth: true,
        }
      );

      if (!json.isSuccess) {
        throw new Error(json.statusMessage || "Failed to complete cart");
      }
      return json.data || false;
    } catch (error) {
      console.error("Failed to complete cart:", error);
      throw new Error("Failed to complete cart");
    }
  }

  async applyCoupon(cartId: number, data: ApplyCouponDto): Promise<CartDto> {
    try {
      const json: ResponseDto<CartDto> = await apiRequest(
        `${API_BASE}/${cartId}/apply-coupon`,
        {
          method: "POST",
          body: JSON.stringify(data),
          requireAuth: true,
        }
      );

      if (!json.isSuccess || !json.data) {
        throw new Error(json.statusMessage || "Failed to apply coupon");
      }
      return json.data;
    } catch (error) {
      console.error("Failed to apply coupon:", error);
      throw new Error("Failed to apply coupon");
    }
  }

  async removeCoupon(cartId: number, couponId: number): Promise<CartDto> {
    try {
      const json: ResponseDto<CartDto> = await apiRequest(
        `${API_BASE}/${cartId}/remove-coupon/${couponId}`,
        {
          method: "DELETE",
          requireAuth: true,
        }
      );

      if (!json.isSuccess || !json.data) {
        throw new Error(json.statusMessage || "Failed to remove coupon");
      }
      return json.data;
    } catch (error) {
      console.error("Failed to remove coupon:", error);
      throw new Error("Failed to remove coupon");
    }
  }

  async checkout(
    cartId: number,
    data: CheckoutRequest
  ): Promise<CheckoutResponse> {
    const url = `${API_BASE}/${cartId}/checkout`;

    try {
      const res: ResponseDto<any>  = await apiRequest(url, {
        method: "POST",
        requireAuth: true,
        body: JSON.stringify(data),
      });

      if (!res.statusCode) {
        const errorJson = await res.data.json().catch(() => null);
        throw new Error(
          errorJson?.statusMessage || `Failed to checkout: ${res.statusMessage}`
        );
      }

      const json: ResponseDto<any> = await res.data.json();

      if (!json.isSuccess || !json.data) {
        throw new Error(json.statusMessage || "Failed to checkout");
      }

      return {
        id: json.data.id,
        orderNumber: json.data.referenceNumber || json.data.id.toString(),
        total: json.data.total,
        status: json.data.status,
      };
    } catch (error) {
      console.error("OrderService.checkout error:", error);
      throw error;
    }
  }
}