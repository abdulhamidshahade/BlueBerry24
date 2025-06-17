## 🌐 API Endpoints

The backend API provides comprehensive endpoints for all e-commerce operations. All endpoints return standardized `ResponseDto` objects with `IsSuccess`, `StatusCode`, `StatusMessage`, `Data`, and `Errors` fields.

### 🔐 Authentication (`/api/auth`)

#### User Management
- `POST /api/auth/register` - User registration (Anonymous)
- `POST /api/auth/login` - User login (Anonymous)
- `POST /api/auth/logout` - User logout (Authenticated)
- `GET /api/auth/me` - Get current user profile (Authenticated)
- `POST /api/auth/refresh-token` - Refresh JWT token (Admin+)

#### User Administration (Admin+)
- `GET /api/auth/users` - Get all users
- `GET /api/auth/users/{id}` - Get user by ID
- `POST /api/auth/users` - Create new user
- `PUT /api/auth/users/{userId}` - Update user details
- `DELETE /api/auth/users/{userId}` - Delete user account

#### User Account Management (Admin+)
- `POST /api/auth/users/{userId}/lock` - Lock user account
- `POST /api/auth/users/{userId}/unlock` - Unlock user account
- `POST /api/auth/users/{userId}/reset-password` - Reset user password
- `POST /api/auth/users/{userId}/verify-email` - Verify user email

#### User Verification
- `GET /api/auth/exists/{id}` - Check if user exists by ID (Admin+)
- `GET /api/auth/exists/email-address/{emailAddress}` - Check if user exists by email (Admin+)

---

### 🛍️ Products (`/api/products`)

#### Product Management
- `GET /api/products` - Get all products (Anonymous)
- `GET /api/products/{id}` - Get product by ID (Anonymous)
- `GET /api/products/name/{name}` - Get product by name (Anonymous)
- `POST /api/products` - Create new product (Admin+) - *Query params: categories (List<int>)*
- `PUT /api/products/{id}` - Update product (Admin+) - *Query params: categories (List<int>)*
- `DELETE /api/products/{id}` - Delete product (Admin+)

#### Product Verification (Admin+)
- `GET /api/products/exists/{id}` - Check if product exists by ID
- `GET /api/products/exists/name/{name}` - Check if product exists by name

---

### 🏷️ Categories (`/api/categories`)

#### Category Management
- `GET /api/categories` - Get all categories (Anonymous)
- `GET /api/categories/{id}` - Get category by ID (Anonymous)
- `GET /api/categories/name/{name}` - Get category by name (Anonymous)
- `POST /api/categories` - Create new category (Admin+)
- `PUT /api/categories/{id}` - Update category (Admin+)
- `DELETE /api/categories/{id}` - Delete category (Admin+)

#### Category Verification (Admin+)
- `GET /api/categories/exists-by-id/{id}` - Check if category exists by ID
- `GET /api/categories/exists-by-name/{name}` - Check if category exists by name

---

### 🛒 Shopping Cart (`/api/shoppingcarts`)

#### Cart Management
- `GET /api/shoppingcarts/user-id` - Get cart by current user ID (Authenticated)
- `GET /api/shoppingcarts/session/{sessionId}` - Get cart by session ID
- `GET /api/shoppingcarts/{cartId}` - Get cart by cart ID
- `POST /api/shoppingcarts/create/{sessionId}` - Create new cart

#### Cart Items
- `POST /api/shoppingcarts/add-item` - Add item to cart
  ```json
  { "cartId": 1, "productId": 1, "quantity": 2 }
  ```
- `PUT /api/shoppingcarts/{cartId}/update` - Update item quantity
- `DELETE /api/shoppingcarts/{cartId}/remove/{productId}` - Remove item from cart
- `DELETE /api/shoppingcarts/{cartId}/clear` - Clear entire cart
- `GET /api/shoppingcarts/{cartId}/item/{productId}` - Get specific cart item

#### Cart Operations
- `POST /api/shoppingcarts/{cartId}/complete` - Mark cart as completed
- `POST /api/shoppingcarts/{cartId}/apply-coupon` - Apply coupon to cart
  ```json
  { "couponCode": "SAVE20" }
  ```
- `DELETE /api/shoppingcarts/{cartId}/remove-coupon/{couponId}` - Remove coupon from cart

#### Checkout
- `POST /api/shoppingcarts/{cartId}/checkout` - Process cart checkout
  ```json
  {
    "firstName": "John",
    "lastName": "Doe",
    "email": "john@example.com",
    "phone": "+1234567890",
    "address": "123 Main St",
    "city": "Springfield",
    "state": "IL",
    "zipCode": "62701",
    "cardNumber": "4111111111111111",
    "expiryDate": "12/25",
    "cvv": "123",
    "cardName": "John Doe",
    "userId": 1
  }
  ```

---

### 📋 Orders (`/api/orders`)

#### Order Management
- `POST /api/orders` - Create order from cart
  ```json
  { "cartId": 1, "notes": "Optional order notes" }
  ```
- `GET /api/orders/{id}` - Get order by ID
- `GET /api/orders/user/{sessionId}` - Get user orders with pagination (*Query: page, pageSize*)
- `GET /api/orders/reference/{referenceNumber}` - Get order by reference number (Admin+)

#### Order Processing
- `PUT /api/orders/{orderId}/process` - Process order (Authenticated)
- `PUT /api/orders/{orderId}/update-status` - Update order status (Admin+)
  ```json
  { "newStatus": "Shipped" }
  ```
- `PUT /api/orders/{orderId}/cancel` - Cancel order (Admin+)
  ```json
  { "reason": "Customer request" }
  ```
- `PUT /api/orders/{orderId}/refund` - Refund order (Admin+)
  ```json
  { "reason": "Product defect" }
  ```
- `PUT /api/orders/{orderId}/mark-paid` - Mark order as paid (Admin+)
  ```json
  { "paymentTransactionId": 123, "paymentProvider": "Stripe" }
  ```

#### Order Analytics (Admin+)
- `GET /api/orders/status/{status}` - Get orders by status (*Query: page, pageSize*)
- `GET /api/orders/admin/all` - Get all orders for admin (*Query: page, pageSize*)
- `GET /api/orders/calculate-totals` - Calculate order totals (*Query: cartId*)

---

### 🎫 Coupons (`/api/coupons`)

#### Coupon Management
- `GET /api/coupons` - Get all coupons (Admin+)
- `GET /api/coupons/{id}` - Get coupon by ID (User+)
- `GET /api/coupons/code/{code}` - Get coupon by code (User+)
- `POST /api/coupons` - Create new coupon (Admin+)
- `PUT /api/coupons/{id}` - Update coupon (Admin+)
- `DELETE /api/coupons/{id}` - Delete coupon (Admin+)

#### Coupon Verification
- `GET /api/coupons/exists-by-id/{id}` - Check if coupon exists by ID (User+)
- `GET /api/coupons/exists-by-code/{code}` - Check if coupon exists by code (User+)

#### User-Coupon Management (Admin+)
- `POST /api/coupons/users/{userId}/coupons` - Add coupon to specific user
- `PUT /api/coupons/users/{userId}/coupons/{couponId}/disable` - Disable user's coupon
- `GET /api/coupons/users/{userId}/coupons` - Get user's coupons (User+)
- `GET /api/coupons/{couponId}/users` - Get users with specific coupon (Admin+)
- `GET /api/coupons/users/{userId}/coupons/{couponCode}/used` - Check if user used coupon (User+)

#### Bulk Coupon Operations (Admin+)
- `POST /api/coupons/add-coupon-to-specific-users/{couponId}` - Add coupon to specific users (*Query: UserIds*)
- `POST /api/coupons/add-coupon-to-all-users/{couponId}` - Add coupon to all users
- `POST /api/coupons/add-coupon-to-new-users/{couponId}` - Add coupon to new users only

---

### ❤️ Wishlist (`/api/wishlists`)

#### Wishlist Management (User+)
- `GET /api/wishlists` - Get user's wishlists
- `GET /api/wishlists/{id}` - Get specific wishlist
- `GET /api/wishlists/default` - Get user's default wishlist
- `GET /api/wishlists/summary` - Get user's wishlist summary
- `POST /api/wishlists` - Create new wishlist
- `PUT /api/wishlists/{id}` - Update wishlist
- `DELETE /api/wishlists/{id}` - Delete wishlist

#### Wishlist Items (User+)
- `POST /api/wishlists/items/add` - Add item to wishlist
  ```json
  { "productId": 1, "wishlistId": 1, "notes": "Optional notes" }
  ```
- `PUT /api/wishlists/{wishlistId}/items/{productId}` - Update wishlist item
- `DELETE /api/wishlists/{wishlistId}/items/{productId}` - Remove item from wishlist
- `GET /api/wishlists/check-product/{productId}` - Check if product is in any wishlist

#### Bulk Operations (User+)
- `POST /api/wishlists/{wishlistId}/items/bulk-add` - Add multiple items
  ```json
  [1, 2, 3, 4, 5]
  ```
- `DELETE /api/wishlists/{wishlistId}/items/bulk-remove` - Remove multiple items
  ```json
  [1, 2, 3]
  ```

#### Wishlist Sharing (User+)
- `PUT /api/wishlists/{wishlistId}/share` - Set wishlist visibility
  ```json
  true
  ```
- `POST /api/wishlists/{wishlistId}/duplicate` - Duplicate wishlist
  ```json
  "My New Wishlist"
  ```
- `DELETE /api/wishlists/{wishlistId}/clear` - Clear all items from wishlist

#### Admin Wishlist Management (Admin+)
- `GET /api/wishlists/admin/all` - Get all wishlists for admin
- `GET /api/wishlists/admin/stats` - Get global wishlist statistics
- `DELETE /api/wishlists/admin/{id}` - Admin delete wishlist
- `DELETE /api/wishlists/admin/{id}/clear` - Admin clear wishlist
- `PUT /api/wishlists/admin/{id}/visibility` - Admin toggle wishlist visibility

---

### 📦 Inventory (`/api/inventories`)

#### Stock Checking
- `GET /api/inventories/check-stock/{productId}/{quantity}` - Check if specific quantity is in stock
- `GET /api/inventories/check-stock/{productId}` - Check stock availability (*Query: quantity=1*)
- `GET /api/inventories/product/{productId}` - Get product with stock information
- `GET /api/inventories/low-stock` - Get low stock products (*Query: limit=50*)
- `GET /api/inventories/history/{productId}` - Get inventory history (*Query: limit=50*)

#### Stock Management
- `POST /api/inventories/reserve-stock` - Reserve stock for order
  ```json
  {
    "productId": 1,
    "quantity": 2,
    "referenceId": 1001,
    "referenceType": "Order"
  }
  ```
- `POST /api/inventories/release-reserved-stock` - Release reserved stock
- `POST /api/inventories/confirm-deduction` - Confirm stock deduction
- `POST /api/inventories/add-stock` - Add stock to product (Authenticated)
  ```json
  {
    "productId": 1,
    "quantity": 100,
    "notes": "New shipment received",
    "performedByUserId": 1
  }
  ```
- `PUT /api/inventories/adjust-stock` - Adjust stock quantity (Authenticated)
  ```json
  {
    "productId": 1,
    "newQuantity": 150,
    "notes": "Inventory correction",
    "performedByUserId": 1
  }
  ```

#### Notifications
- `POST /api/inventories/process-notifications` - Process stock notifications (Authenticated)

---

### 🏪 Shops (`/api/shops`)

#### Shop Management
- `GET /api/shops/{id}` - Get shop by ID
- `PUT /api/shops/{id}` - Update shop information

---

### 👥 Role Management (`/api/rolemanagement`)

*All endpoints require SuperAdmin role*

#### Role Operations
- `POST /api/rolemanagement/roles` - Create new role
  ```json
  "Manager"
  ```
- `DELETE /api/rolemanagement/roles/{roleName}` - Delete role
- `PUT /api/rolemanagement/roles/{oldRoleName}` - Update role name
  ```json
  "Senior Manager"
  ```
- `GET /api/rolemanagement/roles` - Get all roles

#### User-Role Assignment
- `POST /api/rolemanagement/users/{userId}/roles/{roleName}` - Assign role to user
- `DELETE /api/rolemanagement/users/{userId}/roles/{roleName}` - Remove role from user
- `GET /api/rolemanagement/users/{userId}/roles` - Get user's roles
- `GET /api/rolemanagement/users/{userId}/roles/{roleName}/check` - Check if user has role

#### Role Queries
- `GET /api/rolemanagement/roles/{roleName}/users` - Get users in specific role
- `GET /api/rolemanagement/users` - Get all users
- `GET /api/rolemanagement/users/{userId}` - Get specific user
- `GET /api/rolemanagement/stats` - Get role statistics

#### Bulk Operations
- `POST /api/rolemanagement/bulk-assign` - Bulk assign roles to users
  ```json
  {
    "userIds": [1, 2, 3],
    "roleName": "Manager"
  }
  ```
- `POST /api/rolemanagement/initialize-default-roles` - Initialize default system roles
