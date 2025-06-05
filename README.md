# 🫐 BlueBerry24

A modern, full-stack **e-commerce platform** built with **.NET 9** and **Next.js 15**. BlueBerry24 combines a robust, clean architecture backend with a beautiful, responsive React frontend to deliver a comprehensive online shopping experience.

[![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![Next.js 15](https://img.shields.io/badge/Next.js-15.1.8-000000?style=flat&logo=next.js)](https://nextjs.org/)
[![TypeScript](https://img.shields.io/badge/TypeScript-5.0-3178C6?style=flat&logo=typescript)](https://www.typescriptlang.org/)
[![Bootstrap 5](https://img.shields.io/badge/Bootstrap-5.3.6-7952B3?style=flat&logo=bootstrap)](https://getbootstrap.com/)

---

## 🏗️ Architecture Overview

BlueBerry24 follows **Clean Architecture** principles with clear separation of concerns across multiple layers:

### Backend (.NET 9)
| Project | Responsibility |
|---------|----------------|
| **BlueBerry24.Domain**        | Enterprise rules, entities, value objects & validations. Core business logic with no external dependencies except cross-cutting libraries (FluentValidation, Identity, Redis). |
| **BlueBerry24.Application**   | Application logic, use cases, DTOs, services, and AutoMapper profiles. Depends only on Domain layer. |
| **BlueBerry24.Infrastructure**| External concerns: Entity Framework Core, SQL Server persistence, Identity stores, Redis cache, repository implementations. Depends on Domain layer. |
| **BlueBerry24.API**           | RESTful API endpoints using ASP.NET Core Minimal APIs. Exposes OpenAPI/Swagger documentation. |

### Frontend (Next.js 15)
| Directory | Responsibility |
|-----------|----------------|
| **src/app/**              | App Router pages, layouts, and API routes |
| **src/components/**       | Reusable React components (admin, auth, product, etc.) |
| **src/lib/services/**     | API client services for backend communication |
| **src/lib/actions/**      | Server actions for form handling and data mutations |
| **src/types/**            | TypeScript type definitions |

---

## ✨ Features

### 🛒 E-Commerce Core
- **Product Management**: Complete operations with categories, inventory tracking, and search
- **Shopping Cart**: Session-based cart with real-time updates and persistence
- **Order Management**: Full order lifecycle from checkout to fulfillment
- **Wishlist System**: Save products for later with sharing capabilities
- **Coupon System**: Flexible discount codes with various rules and restrictions
- **Inventory Tracking**: Real-time stock management with low-stock alerts

### 🔐 Authentication & Authorization  
- **ASP.NET Identity**: Custom `ApplicationUser` and role-based authorization
- **JWT Authentication**: Secure token-based authentication for API access
- **Role Management**: Multi-level access control (User, Admin, SuperAdmin)
- **User Profile Management**: Complete profile editing and preferences

### 📊 Admin Dashboard
- **Sales Analytics**: Revenue tracking, order statistics, and performance metrics
- **Product Management**: Bulk operations, category management, and inventory control
- **Customer Management**: User accounts, order history, and customer analytics
- **Coupon Management**: Create, manage, and track promotional campaigns
- **Order Tracking**: Real-time order status and fulfillment management
- **System Settings**: Configurable site settings and preferences

### 🎨 Modern UI/UX
- **Responsive Design**: Mobile-first Bootstrap 5 components
- **Server-Side Rendering**: Next.js App Router with optimized performance
- **Interactive Components**: Real-time cart updates and dynamic content
- **Admin Interface**: Comprehensive dashboard with data visualization
- **Toast Notifications**: User feedback for all actions

### 🚀 Technical Features
- **Clean Architecture**: Maintainable, testable, and scalable codebase
- **Entity Framework Core**: Code-first database approach with migrations
- **Redis Caching**: Performance optimization for frequently accessed data
- **FluentValidation**: Comprehensive input validation and error handling
- **AutoMapper**: Efficient object mapping between layers
- **OpenAPI/Swagger**: Complete API documentation and testing interface

---

## 🛠 Tech Stack

### Backend
- **.NET 9** - Latest .NET framework with performance improvements
- **ASP.NET Core 9** - Web API framework with Minimal APIs
- **Entity Framework Core 9** - Object-relational mapping with SQL Server
- **FluentValidation 11** - Robust model validation
- **AutoMapper** - Object-to-object mapping
- **ASP.NET Identity** - Authentication and authorization
- **JWT Bearer** - Token-based authentication
- **Swashbuckle** - OpenAPI documentation

### Frontend
- **Next.js 15** - React framework with App Router
- **React 19** - Latest React with concurrent features
- **TypeScript 5** - Type-safe JavaScript development
- **Bootstrap 5.3** - Responsive CSS framework
- **React Bootstrap** - Bootstrap components for React
- **Bootstrap Icons** - Comprehensive icon library
- **date-fns** - Modern date utility library

---

## 📁 Project Structure

```text
BlueBerry24/
├── BlueBerry24.API/                    # ASP.NET Core Web API
│   ├── Controllers/                    # API Controllers
│   ├── Program.cs                      # Application entry point
│   └── appsettings.json               # Configuration
├── BlueBerry24.Application/            # Application Layer
│   ├── Services/                       # Business logic services
│   │   ├── Concretes/                 # Service implementations
│   │   └── Interfaces/                # Service contracts
│   ├── Dtos/                          # Data Transfer Objects
│   ├── Mapping/                       # AutoMapper profiles
│   └── Authorization/                 # Authorization policies
├── BlueBerry24.Domain/                 # Domain Layer
│   ├── Entities/                      # Domain entities
│   │   ├── AuthEntities/              # User & Role entities
│   │   ├── ProductEntities/           # Product & Category entities
│   │   ├── OrderEntities/             # Order management entities
│   │   ├── ShoppingCartEntities/      # Cart entities
│   │   ├── CouponEntities/            # Discount entities
│   │   ├── WishlistEntities/          # Wishlist entities
│   │   ├── ShopEntities/              # Shop/Store entities
│   │   └── InventoryEntities/         # Stock management entities
│   ├── Repositories/                  # Repository interfaces
│   └── Constants/                     # Domain constants
├── BlueBerry24.Infrastructure/         # Infrastructure Layer
│   ├── Data/                          # EF Core DbContext
│   ├── Repositories/                  # Repository implementations
│   └── Migrations/                    # Database migrations
└── BlueBerry24.Web/                   # Next.js Frontend
    ├── src/
    │   ├── app/                       # App Router pages
    │   │   ├── admin/                 # Admin dashboard pages
    │   │   │   ├── analytics/         # Analytics & reports
    │   │   │   ├── categories/        # Category management
    │   │   │   ├── coupons/           # Coupon management
    │   │   │   ├── customers/         # Customer management
    │   │   │   ├── inventory/         # Inventory management
    │   │   │   ├── orders/            # Order management
    │   │   │   ├── products/          # Product management
    │   │   │   ├── reports/           # Business reports
    │   │   │   ├── settings/          # System settings
    │   │   │   └── users/             # User management
    │   │   ├── auth/                  # Authentication pages
    │   │   ├── cart/                  # Shopping cart
    │   │   ├── categories/            # Product categories
    │   │   ├── checkout/              # Checkout process
    │   │   ├── orders/                # Order history
    │   │   ├── products/              # Product catalog
    │   │   └── profile/               # User profile
    │   ├── components/                # React components
    │   │   ├── admin/                 # Admin components
    │   │   ├── auth/                  # Authentication components
    │   │   ├── cart/                  # Shopping cart components
    │   │   ├── product/               # Product components
    │   │   └── ui/                    # Common UI components
    │   ├── lib/
    │   │   ├── services/              # API client services
    │   │   ├── actions/               # Server actions
    │   │   └── utils/                 # Utility functions
    │   └── types/                     # TypeScript definitions
    ├── public/                        # Static assets
    └── package.json                   # Dependencies
```

---

## ⚡️ Quick Start

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/) (Preview Channel)
- [Node.js 18+](https://nodejs.org/) and npm/yarn
- SQL Server (LocalDB or full instance)
- Redis (optional, for caching)

### 1. Clone the Repository
```bash
git clone https://github.com/abdulhamidshahade/BlueBerry24.git
cd BlueBerry24
```

### 2. Backend Setup (.NET API)

**Install dependencies:**
```bash
dotnet restore
```

**Configure database connection:**
```bash
# Using user secrets (recommended for development)
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.;Database=BlueBerry24;Trusted_Connection=True;MultipleActiveResultSets=true" --project BlueBerry24.API

# Or edit appsettings.Development.json directly
```

**Run database migrations:**
```bash
dotnet ef database update --startup-project BlueBerry24.API --project BlueBerry24.Infrastructure
```

**Start the API:**
```bash
dotnet run --project BlueBerry24.API
```
API will be available at `https://localhost:7105` with Swagger at `/swagger`

### 3. Frontend Setup (Next.js)

**Navigate to frontend directory:**
```bash
cd BlueBerry24.Web
```

**Install dependencies:**
```bash
npm install
# or
yarn install
```

**Configure environment variables:**
```bash
# Create .env.local file
echo "NEXT_PUBLIC_API_URL=https://localhost:3000" > .env.local
```

**Start development server:**
```bash
npm run dev
# or
yarn dev
```
Frontend will be available at `http://localhost:3000`

### 4. Default Admin Account
After running the database migrations, you can create an admin account through the registration page and promote it via the database or create one programmatically.

---

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

---

## 🔒 Authorization Levels

The API uses attribute-based authorization with the following levels:

- **Anonymous** - No authentication required
- **User+** (`[UserAndAbove]`) - Requires User role or higher
- **Admin+** (`[AdminAndAbove]`) - Requires Admin role or higher  
- **SuperAdmin** (`[SuperAdminOnly]`) - Requires SuperAdmin role only
- **Authenticated** (`[AllRoles]`) - Any authenticated user

## 📊 Response Format

All API responses follow a standardized format:

```json
{
  "isSuccess": true,
  "statusCode": 200,
  "statusMessage": "Operation completed successfully",
  "data": { /* Response data */ },
  "errors": [ /* Error messages if any */ ]
}
```

## 🔧 Query Parameters

Many endpoints support query parameters for pagination and filtering:

- `page` - Page number (default: 1)
- `pageSize` - Items per page (default: 10-50 depending on endpoint)
- `limit` - Maximum items to return

Visit `/swagger` when running the API to see the complete OpenAPI documentation with request/response schemas and interactive testing interface.

---

## 🎨 Frontend Pages

### Public Pages
- **Home** (`/`) - Landing page with featured products and categories
- **Products** (`/products`) - Product catalog with search and filtering
- **Product Details** (`/products/[id]`) - Individual product page
- **Categories** (`/categories`) - Browse by category
- **Cart** (`/cart`) - Shopping cart management
- **Checkout** (`/checkout`) - Order placement process

### User Dashboard
- **Profile** (`/profile`) - User account management
- **Orders** (`/orders`) - Order history and tracking
- **Wishlist** (`/profile/wishlist`) - Saved products

### Admin Dashboard
- **Overview** (`/admin`) - Sales metrics and quick actions
- **Products** (`/admin/products`) - Product management
- **Categories** (`/admin/categories`) - Category management
- **Orders** (`/admin/orders`) - Order management
- **Customers** (`/admin/customers`) - Customer management
- **Coupons** (`/admin/coupons`) - Discount code management
- **Analytics** (`/admin/analytics`) - Business intelligence
- **Inventory** (`/admin/inventory`) - Stock management
- **Settings** (`/admin/settings`) - System configuration

---

## 🔧 Configuration

### Backend Configuration
Key settings in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your SQL Server connection string"
  },
  "Redis": {
    "ConnectionString": "localhost:6379"
  },
  "Jwt": {
    "Key": "your-secret-key",
    "Issuer": "BlueBerry24",
    "Audience": "BlueBerry24-Users",
    "ExpirationHours": 24
  }
}
```

### Frontend Configuration
Environment variables in `.env.local`:

```bash
NEXT_PUBLIC_API_URL=https://localhost:3000
NEXT_PUBLIC_SITE_NAME=BlueBerry24
```

---

## 🧪 Testing

Unit and integration tests are planned for both backend and frontend components. When implemented, they will include:

- **Backend**: xUnit tests for services, repositories, and controllers
- **Frontend**: Jest and React Testing Library for components
- **E2E**: Playwright tests for critical user journeys

---

## 🚀 Deployment

### Backend Deployment
1. Publish the API:
   ```bash
   dotnet publish BlueBerry24.API -c Release -o ./publish
   ```
2. Deploy to your preferred hosting platform (Azure App Service, AWS, etc.)
3. Configure production connection strings and secrets

### Frontend Deployment
1. Build the Next.js application:
   ```bash
   cd BlueBerry24.Web
   npm run build
   ```
2. Deploy to Vercel, Netlify, or your preferred hosting platform
3. Configure environment variables for production API URL

### Database Deployment
```bash
# Generate migration scripts for production
dotnet ef migrations script --project BlueBerry24.Infrastructure --startup-project BlueBerry24.API
```

---

## 🤝 Contributing

We welcome contributions! Please follow these steps:

1. Fork the repository
2. Create your feature branch: `git checkout -b feature/amazing-feature`
3. Commit your changes: `git commit -m 'feat: add amazing feature'`
4. Push to the branch: `git push origin feature/amazing-feature`
5. Open a Pull Request

### Development Guidelines
- Follow Clean Architecture principles
- Write descriptive commit messages using conventional commits
- Ensure code is properly formatted and linted
- Add appropriate tests for new features
- Update documentation as needed

---

## 📖 Documentation

- **API Documentation**: Available at `/swagger` when running the API
- **Component Documentation**: Storybook integration planned
- **Architecture Decision Records**: Located in `/docs/adr/` (planned)

---

## 🛡️ Security

BlueBerry24 implements comprehensive security measures:

- **Authentication**: JWT tokens with secure headers
- **Authorization**: Role-based access control
- **Data Protection**: ASP.NET Core Data Protection APIs
- **Input Validation**: FluentValidation with XSS protection
- **HTTPS Enforcement**: Secure communication only
- **CORS Policy**: Configured for frontend domain only

---

## 📊 Performance

- **Caching**: Redis distributed cache for improved response times
- **Database**: Optimized EF Core queries with proper indexing
- **Frontend**: Next.js SSR and static generation for fast loading
- **Images**: Optimized image loading and responsive images
- **Bundle Size**: Tree shaking and code splitting for minimal bundles

---

## 📄 License

Distributed under the MIT License. See `LICENSE.txt` for more information.

---

## 🙏 Acknowledgments

- [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/) - Web framework
- [Next.js](https://nextjs.org/) - React framework
- [Bootstrap](https://getbootstrap.com/) - CSS framework
- [Bootstrap Icons](https://icons.getbootstrap.com/) - Icon library
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) - ORM
- [FluentValidation](https://fluentvalidation.net/) - Validation library

---

## 📞 Support

For support and questions:
- 📧 Email: shahade.abdulhamid@gmail.com

---
