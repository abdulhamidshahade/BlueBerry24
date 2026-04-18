# 🫐 Berryfy

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
| **Berryfy.Domain**        | Enterprise rules, entities, value objects & validations. Core business logic with no external dependencies except cross-cutting libraries (FluentValidation, Identity, Redis). |
| **Berryfy.Application**   | Application logic, use cases, DTOs, services, and AutoMapper profiles. Depends only on Domain layer. |
| **Berryfy.Infrastructure**| External concerns: Entity Framework Core, SQL Server persistence, Identity stores, Redis cache, repository implementations. Depends on Domain layer. |
| **Berryfy.API**           | RESTful API endpoints using ASP.NET Core Minimal APIs. Exposes OpenAPI/Swagger documentation. |

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
Berryfy/
├── Berryfy.API/                    # ASP.NET Core Web API
│   ├── Controllers/                    # API Controllers
│   ├── Program.cs                      # Application entry point
│   └── appsettings.json               # Configuration
├── Berryfy.Application/            # Application Layer
│   ├── Services/                       # Business logic services
│   │   ├── Concretes/                 # Service implementations
│   │   └── Interfaces/                # Service contracts
│   ├── Dtos/                          # Data Transfer Objects
│   ├── Mapping/                       # AutoMapper profiles
│   └── Authorization/                 # Authorization policies
├── Berryfy.Domain/                 # Domain Layer
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
├── Berryfy.Infrastructure/         # Infrastructure Layer
│   ├── Data/                          # EF Core DbContext
│   ├── Repositories/                  # Repository implementations
│   └── Migrations/                    # Database migrations
└── Berryfy.Web/                   # Next.js Frontend
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
git clone https://github.com/abdulhamidshahade/berryfy.git berryfy
cd berryfy
```

### 2. Backend Setup (.NET API)

**Install dependencies:**
```bash
dotnet restore
```

**Configure database connection:**
```bash
# Using user secrets (recommended for development)
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.;Database=Berryfy;Trusted_Connection=True;MultipleActiveResultSets=true" --project Berryfy.API

# Or edit appsettings.Development.json directly
```

**Run database migrations:**
```bash
dotnet ef database update --startup-project Berryfy.API --project Berryfy.Infrastructure
```

**Start the API:**
```bash
dotnet run --project Berryfy.API
```
API will be available at `https://localhost:7105` with Swagger at `/swagger`

### 3. Frontend Setup (Next.js)

**Navigate to frontend directory:**
```bash
cd Berryfy.Web
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

### 5. Run with Docker (optional)

From the repository root:

```bash
# Copy environment template and add your secrets (do not commit .env)
cp .env.example .env
# Edit .env with your SA_PASSWORD, JWT secret, and Gmail credentials

# Build and start all services
docker compose up -d
```

- **API**: http://localhost:7105  
- **Frontend**: http://localhost:30305  
- **SQL Server**: localhost:11433 (user `sa`, password from `.env`)

Secrets (passwords, JWT key, Gmail) are read from `.env`; only `.env.example` is committed.

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
    "Issuer": "Berryfy",
    "Audience": "berryfy-Users",
    "ExpirationHours": 24
  }
}
```

### Frontend Configuration
Environment variables in `.env.local`:

```bash
NEXT_PUBLIC_API_URL=https://localhost:3000
NEXT_PUBLIC_SITE_NAME=Berryfy
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
   dotnet publish Berryfy.API -c Release -o ./publish
   ```
2. Deploy to your preferred hosting platform (Azure App Service, AWS, etc.)
3. Configure production connection strings and secrets

### Frontend Deployment
1. Build the Next.js application:
   ```bash
   cd Berryfy.Web
   npm run build
   ```
2. Deploy to Vercel, Netlify, or your preferred hosting platform
3. Configure environment variables for production API URL

### Database Deployment
```bash
# Generate migration scripts for production
dotnet ef migrations script --project Berryfy.Infrastructure --startup-project Berryfy.API
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

Berryfy implements comprehensive security measures:

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
- 📧 Email: support@berryfy.org

---
