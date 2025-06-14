"use server";

import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import { CartService } from "../services/cart/service";
import { ICartService } from "../services/cart/interface";
import { cookies } from "next/headers";
import { ProductService } from "@/lib/services/product/service";
import { AddToCartDto } from "@/types/cart";

const cartService: ICartService = new CartService();
const productService = new ProductService();

export async function getOrCreateCart() {
  const cookieStore = cookies();

  const sessionId = (await cookieStore).get("cart_session")?.value;
  let cart = null;

  if (sessionId) {
    cart = await cartService.getBySessionId(sessionId);
  } else {
    cart = await cartService.getByUserId();
  }

  if(!cart){
    cart = cartService.create();
  }
  return cart;
}

export async function getCart() {
  try {
    return await getOrCreateCart();
  } catch (error) {
    console.error("Error getting cart:", error);
    return null;
  }
}

export async function addToCart(formData: FormData) {
  try {
    const productId = parseInt(formData.get("productId") as string);
    const quantity = parseInt(formData.get("quantity") as string) || 1;

    const product = await productService.getById(productId);
    if (!product || !product.isActive) {
      throw new Error("Product not available");
    }

    if (product.stockQuantity <= product.reservedStock + quantity) {
      throw new Error("Insufficient stock");
    }

    const cart = await getOrCreateCart();

    const itemToAdd: AddToCartDto = {
      quantity: quantity,
      sessionId: cart.sessionId,
      productId: productId,
      userId: cart.userId,
      cartId: cart.id,
    };

    await cartService.addItem(itemToAdd);

    revalidatePath("/cart");
    revalidatePath("/products");
  } catch (error) {
    console.error("Error adding to cart:", error);
    throw new Error(
      error instanceof Error ? error.message : "Failed to add item to cart"
    );
  }
}

export async function updateCartItem(formData: FormData) {
  try {
    const productId = parseInt(formData.get("productId") as string);
    const quantity = parseInt(formData.get("quantity") as string);

    if (quantity < 0) {
      throw new Error("Quantity cannot be negative");
    }

    const cart = await getOrCreateCart();

    if (quantity === 0) {
      await cartService.removeItem(cart.id, productId);
    } else {
      const product = await productService.getById(productId);
      if (product && product.stockQuantity < product.reservedStock + quantity) {
        throw new Error("Insufficient stock");
      }

      const itemToUpdate = {
        productId: productId,
        quantity: quantity,
        userId: cart.userId,
        sessionId: cart.sessionId
      };
      await cartService.updateItemQuantity(cart.id, itemToUpdate);
    }

    revalidatePath("/cart");
  } catch (error) {
    console.error("Error updating cart item:", error);
    throw new Error(
      error instanceof Error ? error.message : "Failed to update cart item"
    );
  }
}

export async function removeFromCart(formData: FormData) {
  try {
    const productId = parseInt(formData.get("productId") as string);

    const cart = await getOrCreateCart();
    await cartService.removeItem(cart.id, productId);

    revalidatePath("/cart");
  } catch (error) {
    console.error("Error removing from cart:", error);
    throw new Error("Failed to remove item from cart");
  }
}

export async function clearCart() {
  try {
    const cart = await getOrCreateCart();
    await cartService.clearCart(cart.id);

    revalidatePath("/cart");
  } catch (error) {
    console.error("Error clearing cart:", error);
    throw new Error("Failed to clear cart");
  }
}

export async function applyCoupon(formData: FormData) {
  try {
    const couponCode = formData.get("couponCode") as string;

    if (!couponCode) {
      throw new Error("Coupon code is required");
    }

    const cart = await getOrCreateCart();
    await cartService.applyCoupon(cart.id, { couponCode });

    revalidatePath("/cart");
  } catch (error) {
    console.error("Error applying coupon:", error);
    throw new Error(
      error instanceof Error ? error.message : "Failed to apply coupon"
    );
  }
}

export async function removeCoupon(formData: FormData) {
  try {
    const couponId = parseInt(formData.get("couponId") as string);

    const cart = await getOrCreateCart();
    await cartService.removeCoupon(cart.id, couponId);

    revalidatePath("/cart");
  } catch (error) {
    console.error("Error removing coupon:", error);
    throw new Error("Failed to remove coupon");
  }
}

export async function proceedToCheckout() {
  const cart = await getOrCreateCart();

  if (!cart.cartItems || cart.cartItems.length === 0) {
    throw new Error("Cart is empty");
  }

  for (const item of cart.cartItems) {
    const product = await productService.getById(item.productId);
    if (!product || !product.isActive) {
      throw new Error(
        `Product ${product?.name || item.productId} is not available`
      );
    }

    if (product.stockQuantity < product.reservedStock + item.quantity) {
      throw new Error(`Insufficient stock for ${product.name}`);
    }
  }

  redirect("/checkout");
}