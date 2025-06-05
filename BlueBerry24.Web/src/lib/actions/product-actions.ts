"use server";

import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import { ProductService } from "@/lib/services/product/service";
import { CreateProductDto, UpdateProductDto } from "@/types/product";
import { IProductService } from "../services/product/interface";
import fs from "fs";
import path from "path";
import { v4 as uuidv4 } from "uuid";

const productService: IProductService = new ProductService();

async function uploadImageFile(
  file: File,
  currentImageUrl?: string
): Promise<string> {
  const useCloudflare = process.env.USE_CLOUDFLARE === "true";

  if (useCloudflare) {
    try {
      const cfRes = await fetch(
        `https://api.cloudflare.com/client/v4/accounts/${process.env.CF_ACCOUNT_ID}/images/v2/direct_upload`,
        {
          method: "POST",
          headers: {
            Authorization: `Bearer ${process.env.CF_API_TOKEN}`,
          },
        }
      );

      const cfJson = await cfRes.json();
      const uploadURL = cfJson.result?.uploadURL;

      if (!uploadURL) {
        throw new Error("Failed to get upload URL from Cloudflare");
      }

      const cloudflareFormData = new FormData();
      cloudflareFormData.append("file", file);

      const uploadRes = await fetch(uploadURL, {
        method: "POST",
        body: cloudflareFormData,
      });

      const data = await uploadRes.json();

      if (!data.result || !data.result.id) {
        throw new Error("Cloudflare upload failed");
      }

      return `https://imagedelivery.net/${process.env.NEXT_PUBLIC_CF_DELIVERY_ID}/${data.result.id}/public`;
    } catch (cloudflareError) {
      console.log(
        "Cloudflare upload failed, falling back to local:",
        cloudflareError
      );
    }
  }

  const bytes = await file.arrayBuffer();
  const buffer = Buffer.from(bytes);

  const fileExtension = file.name.substring(file.name.lastIndexOf("."));

  const fileName = `${Date.now()}_product_${uuidv4()}${fileExtension}`;
  const uploadDir = path.join(process.cwd(), "public/uploads/product");

  if (!fs.existsSync(uploadDir)) {
    fs.mkdirSync(uploadDir, { recursive: true });
  }

  if (currentImageUrl) {
    var fullPath = path.join(process.cwd(), `public${currentImageUrl}`);
    if (fs.existsSync(fullPath)) {
      fs.rmSync(fullPath, { recursive: true });
    }
  }

  const filePath = path.join(uploadDir, fileName);
  fs.writeFileSync(filePath, buffer);

  return `/uploads/product/${fileName}`;
}

export async function createProduct(formData: FormData): Promise<void> {
  try {
    //get -> for one element/ getAll -> for multiple elements
    const categories = formData.getAll("categories");
    const categoryIds = categories
      ? categories
          .map((id) => parseInt(id as string))
          .filter((id) => !isNaN(id))
      : [];
    const imageFile = formData.get("imageFile") as File;

    const errors: Record<string, string> = {};

    if (!imageFile || imageFile.size === 0) {
      errors.imageFile = "Category image is required";
    } else {
      if (!imageFile.type.startsWith("image/")) {
        errors.imageFile = "Please select a valid image file";
      } else if (imageFile.size > 10 * 1024 * 1024) {
        errors.imageFile = "Image size must be less than 10MB";
      }
    }

    const imageUrl = await uploadImageFile(imageFile);

    const productData: CreateProductDto = {
      name: formData.get("name") as string,
      description: formData.get("description") as string,
      price: parseFloat(formData.get("price") as string),
      stockQuantity: parseInt(formData.get("stockQuantity") as string),
      imageUrl: imageUrl,
      reservedStock: parseInt(formData.get("reservedStock") as string) || 0,
      lowStockThreshold:
        parseInt(formData.get("lowStockThreshold") as string) || 10,
      isActive: formData.get("isActive") === "on",
      sku: formData.get("sku") as string,
    };

    await productService.create(productData, categoryIds);

    revalidatePath("/admin/products");
    revalidatePath("/products");
    redirect("/admin/products?success=created");
  } catch (error) {
    if (error instanceof Error && error.message === "NEXT_REDIRECT") {
      throw error;
    }
    console.error("Error creating product:", error);
    throw new Error("Failed to create product");
  }
}

export async function updateProduct(
  formData: FormData,
  currentImageUrl: string
) {
  try {
    const id = parseInt(formData.get("id") as string);
    const categories = formData.getAll("categories");
    const categoryIds = categories
      ? categories
          .map((id) => parseInt(id as string))
          .filter((id) => !isNaN(id))
      : [];

    const imageFile = formData.get("imageFile") as File;

    const imageUrl = await uploadImageFile(imageFile, currentImageUrl);

    const productData: UpdateProductDto = {
      id,
      name: formData.get("name") as string,
      description: formData.get("description") as string,
      price: parseFloat(formData.get("price") as string),
      stockQuantity: parseInt(formData.get("stockQuantity") as string),
      imageUrl: imageUrl,
      reservedStock: parseInt(formData.get("reservedStock") as string) || 0,
      lowStockThreshold:
        parseInt(formData.get("lowStockThreshold") as string) || 10,
      isActive: formData.get("isActive") === "on",
      sku: formData.get("sku") as string,
    };

    await productService.update(id, productData, categoryIds);

    revalidatePath("/admin/products");
    revalidatePath("/products");
    redirect("/admin/products?success=updated");
  } catch (error) {
    if (error instanceof Error && error.message === "NEXT_REDIRECT") {
      throw error;
    }
    console.error("Error updating product:", error);
    throw new Error("Failed to update product");
  }
}

export async function deleteProduct(formData: FormData) {
  try {
    const id = parseInt(formData.get("id") as string);

    const product = await productService.getById(id);

    await productService.delete(id);

    

    if (product && product.imageUrl.startsWith('/uploads/')) {
          try {
            const imagePath = path.join(process.cwd(), 'public', product.imageUrl);
            if (fs.existsSync(imagePath)) {
              fs.unlinkSync(imagePath);
              console.log('Cleaned up image file:', product.imageUrl);
            }
          } catch (cleanupError) {
            console.error('Failed to cleanup image file:', cleanupError);
          }
        }

    revalidatePath("/admin/products");
    revalidatePath("/products");
    redirect("/admin/products?success=deleted");
  } catch (error) {
    if (error instanceof Error && error.message === "NEXT_REDIRECT") {
      throw error;
    }
    console.error("Error deleting product:", error);
    throw new Error("Failed to delete product");
  }
}

export async function getProducts() {
  try {
    return await productService.getAll();
  } catch (error) {
    console.error("Error fetching products:", error);
    return [];
  }
}

export async function getProduct(id: number) {
  try {
    return await productService.getById(id);
  } catch (error) {
    console.error("Error fetching product:", error);
    return null;
  }
}