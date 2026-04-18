# 🫐 BlueBerry24 Web Frontend

A modern, responsive **e-commerce frontend** built with **Next.js 15** and **TypeScript**. This is the client-side application for the BlueBerry24 e-commerce platform, featuring a comprehensive shopping experience with an advanced admin dashboard.

[![Next.js 15](https://img.shields.io/badge/Next.js-15.1.8-000000?style=flat&logo=next.js)](https://nextjs.org/)
[![React 19](https://img.shields.io/badge/React-19.0.0-61DAFB?style=flat&logo=react)](https://reactjs.org/)
[![TypeScript](https://img.shields.io/badge/TypeScript-5.0-3178C6?style=flat&logo=typescript)](https://www.typescriptlang.org/)
[![Bootstrap 5](https://img.shields.io/badge/Bootstrap-5.3.6-7952B3?style=flat&logo=bootstrap)](https://getbootstrap.com/)

---

## 🎯 Overview

BlueBerry24 Web is a feature-rich e-commerce frontend that provides:

- **Customer Experience**: Modern shopping interface with product browsing, cart management, and checkout
- **Admin Dashboard**: Comprehensive management system for products, orders, customers, and analytics
- **Authentication System**: Secure user registration, login, and profile management
- **Responsive Design**: Mobile-first approach with Bootstrap 5
- **Server-Side Rendering**: Next.js App Router for optimal performance and SEO

---

## ✨ Features

### 🛒 Customer Features
- **Product Catalog**: Browse products with categories, search, and filtering
- **Shopping Cart**: Real-time cart updates with persistent storage
- **Checkout Process**: Streamlined checkout with payment integration
- **User Authentication**: Registration, login, email confirmation, and password reset
- **User Profile**: Account management, order history, and preferences
- **Wishlist**: Save products for later purchase
- **Order Tracking**: View order status and history
- **Responsive Design**: Optimized for desktop, tablet, and mobile

### 🎛️ Admin Dashboard
- **Analytics & Reports**: Sales metrics, revenue tracking, and business insights
- **Product Management**: Create, edit, delete products with bulk operations
- **Category Management**: Organize products into hierarchical categories
- **Inventory Management**: Track stock levels and manage inventory
- **Order Management**: Process orders, update status, and handle fulfillment
- **Customer Management**: View customer data, orders, and analytics
- **Coupon Management**: Create and manage discount codes and promotions
- **User Management**: Manage user accounts and roles
- **Role Management**: Configure permissions and access levels
- **System Settings**: Configure site-wide settings and preferences

### 🔐 Authentication & Security
- **JWT Authentication**: Secure token-based authentication
- **Role-Based Access**: Multi-level permissions (User, Admin, SuperAdmin)
- **Protected Routes**: Middleware-based route protection
- **Form Validation**: Client-side and server-side validation
- **CSRF Protection**: Cross-site request forgery prevention

---

## 🛠 Tech Stack

### Core Technologies
- **Next.js 15** - React framework with App Router
- **React 19** - Latest React with concurrent features
- **TypeScript 5** - Type-safe development
- **Bootstrap 5.3** - Responsive CSS framework
- **React Bootstrap 2.10** - Bootstrap components for React

### Key Dependencies
- **Bootstrap Icons** - Comprehensive icon library
- **date-fns** - Modern date utility library
- **UUID** - Unique identifier generation
- **Formidable** - File upload handling

### Development Tools
- **ESLint** - Code linting and formatting
- **TypeScript** - Static type checking
- **CSS Modules** - Scoped styling

---

## 📁 Project Structure

```
BlueBerry24.Web/
├── src/
│   ├── app/                           # Next.js App Router
│   │   ├── admin/                     # Admin Dashboard
│   │   │   ├── analytics/             # Sales analytics and reports
│   │   │   ├── categories/            # Category management
│   │   │   ├── coupons/               # Coupon and discount management
│   │   │   ├── customers/             # Customer data and analytics
│   │   │   ├── inventory/             # Inventory and stock management
│   │   │   ├── orders/                # Order processing and tracking
│   │   │   ├── payments/              # Payment management
│   │   │   ├── products/              # Product catalog management
│   │   │   ├── reports/               # Business reports and insights
│   │   │   ├── role-management/       # User roles and permissions
│   │   │   ├── settings/              # System configuration
│   │   │   ├── Traffic/               # Website traffic analytics
│   │   │   ├── users/                 # User account management
│   │   │   └── wishlists/             # Wishlist management
│   │   ├── api/                       # API routes
│   │   │   ├── auth/                  # Authentication endpoints
│   │   │   └── receipts/              # Receipt generation
│   │   ├── auth/                      # Authentication pages
│   │   │   ├── login/                 # User login
│   │   │   ├── register/              # User registration
│   │   │   ├── forgot-password/       # Password reset
│   │   │   ├── confirm-email/         # Email confirmation
│   │   │   └── resend-confirmation/   # Resend confirmation
│   │   ├── cart/                      # Shopping cart
│   │   ├── categories/                # Product categories
│   │   ├── checkout/                  # Checkout process
│   │   ├── orders/                    # Order history
│   │   ├── payment/                   # Payment processing
│   │   ├── products/                  # Product catalog
│   │   ├── profile/                   # User profile management
│   │   ├── globals.css                # Global styles
│   │   ├── home.css                   # Homepage styles
│   │   ├── layout.tsx                 # Root layout
│   │   └── page.tsx                   # Homepage
│   ├── components/                    # Reusable React components
│   │   ├── admin/                     # Admin-specific components
│   │   ├── auth/                      # Authentication components
│   │   ├── cart/                      # Shopping cart components
│   │   ├── category/                  # Category components
│   │   ├── checkout/                  # Checkout components
│   │   ├── coupon/                    # Coupon components
│   │   ├── layout/                    # Layout components
│   │   ├── product/                   # Product components
│   │   ├── roleManagement/            # Role management components
│   │   └── ui/                        # Common UI components
│   ├── lib/                           # Utility libraries
│   │   ├── actions/                   # Server actions
│   │   ├── services/                  # API client services
│   │   │   ├── auth/                  # Authentication services
│   │   │   ├── cart/                  # Cart services
│   │   │   ├── category/              # Category services
│   │   │   ├── coupon/                # Coupon services
│   │   │   ├── email/                 # Email services
│   │   │   ├── inventory/             # Inventory services
│   │   │   ├── order/                 # Order services
│   │   │   ├── payment/               # Payment services
│   │   │   ├── product/               # Product services
│   │   │   ├── role/                  # Role management services
│   │   │   ├── shop/                  # Shop services
│   │   │   └── wishlist/              # Wishlist services
│   │   └── utils/                     # Utility functions
│   ├── types/                         # TypeScript type definitions
│   │   ├── auth.ts                    # Authentication types
│   │   ├── cart.ts                    # Shopping cart types
│   │   ├── category.ts                # Category types
│   │   ├── coupon.ts                  # Coupon types
│   │   ├── order.ts                   # Order types
│   │   ├── payment.ts                 # Payment types
│   │   ├── product.ts                 # Product types
│   │   ├── shop.ts                    # Shop types
│   │   └── wishlist.ts                # Wishlist types
│   └── middleware.ts                  # Next.js middleware for auth
├── public/                            # Static assets
│   ├── uploads/                       # User uploaded files
│   │   ├── category/                  # Category images
│   │   └── product/                   # Product images
│   └── *.svg                          # Static SVG files
├── next.config.ts                     # Next.js configuration
├── package.json                       # Dependencies and scripts
├── tsconfig.json                      # TypeScript configuration
└── README.md                          # This file
```

---

## ⚡️ Quick Start

### Prerequisites
- **Node.js 18+** and npm/yarn/pnpm
- **BlueBerry24.API** running (for backend integration)
- Modern web browser

### 1. Installation

```bash
# Clone the repository (if not already done)
git clone https://github.com/abdulhamidshahade/BlueBerry24.git
cd BlueBerry24/BlueBerry24.Web

# Install dependencies
npm install
# or
yarn install
# or
pnpm install
```

### 2. Environment Setup

Create environment variables file:

```bash
# .env.local
NEXT_PUBLIC_API_BASE_URL=https://localhost:7001/api
NEXT_PUBLIC_APP_URL=http://localhost:3000
```

### 3. Start Development Server

```bash
npm run dev
# or
yarn dev
# or
pnpm dev
```

Open [http://localhost:3000](http://localhost:3000) in your browser.

---

## 📜 Available Scripts

| Script | Description |
|--------|-------------|
| `npm run dev` | Start development server with hot reload |
| `npm run build` | Build the application for production |
| `npm run start` | Start production server |
| `npm run lint` | Run ESLint for code quality checks |

---

## 🔧 Configuration

### Next.js Configuration

The project includes custom Next.js configuration in `next.config.ts`:

```typescript
const nextConfig: NextConfig = {
  experimental: {
    serverActions: {
      bodySizeLimit: '10mb', // File upload limit
    },
  },
};
```

### Environment Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `NEXT_PUBLIC_API_BASE_URL` | Backend API base URL | `https://localhost:7001/api` |
| `NEXT_PUBLIC_APP_URL` | Frontend application URL | `http://localhost:3000` |

---

## 🔌 API Integration

The frontend communicates with the BlueBerry24.API backend through:

### Service Layer
- **Centralized API clients** in `src/lib/services/`
- **Type-safe requests** with TypeScript interfaces
- **Error handling** and response transformation
- **Authentication** token management

### Server Actions
- **Form handling** with Next.js server actions
- **File uploads** for products and categories
- **Data mutations** with optimistic updates

### Authentication Flow
1. Login/Register through auth services
2. JWT token stored securely
3. Automatic token inclusion in API requests
4. Route protection via middleware

---

## 🎨 Styling & UI

### Design System
- **Bootstrap 5.3** for responsive layouts
- **Bootstrap Icons** for consistent iconography
- **Custom CSS** for brand-specific styling
- **React Bootstrap** components for enhanced functionality

### Responsive Design
- **Mobile-first** approach
- **Breakpoint-based** layouts
- **Touch-friendly** interfaces
- **Accessibility** considerations

---

## 🛡️ Security Features

### Authentication Security
- **JWT token** management
- **HTTP-only cookies** (where applicable)
- **CSRF protection** via middleware
- **Route protection** for authenticated areas

### Input Validation
- **Client-side validation** with TypeScript
- **Server-side validation** integration
- **File upload** security measures
- **XSS prevention** through React's built-in protection

---

## 🚀 Deployment

### Build for Production

```bash
npm run build
npm run start
```

### Environment-Specific Configuration

**Production Environment Variables:**
```bash
NEXT_PUBLIC_API_BASE_URL=https://your-api-domain.com/api
NEXT_PUBLIC_APP_URL=https://your-frontend-domain.com
```

### Recommended Deployment Platforms
- **Vercel** (recommended for Next.js)
- **Netlify**
- **AWS Amplify**
- **Azure Static Web Apps**
- **Custom Docker container**

---

## 📚 Documentation

### Key Pages & Features

#### Customer Pages
- **Homepage** (`/`) - Product showcase and hero section
- **Product Catalog** (`/products`) - Browse all products
- **Product Details** (`/products/[id]`) - Individual product pages
- **Categories** (`/categories`) - Browse by category
- **Shopping Cart** (`/cart`) - Review items before checkout
- **Checkout** (`/checkout`) - Complete purchase process
- **User Profile** (`/profile`) - Account management

#### Admin Dashboard
- **Dashboard** (`/admin`) - Overview and quick stats
- **Products** (`/admin/products`) - Manage product catalog
- **Categories** (`/admin/categories`) - Organize product categories
- **Orders** (`/admin/orders`) - Process and track orders
- **Customers** (`/admin/customers`) - Customer management
- **Analytics** (`/admin/analytics`) - Business insights
- **Inventory** (`/admin/inventory`) - Stock management
- **Coupons** (`/admin/coupons`) - Promotional campaigns

### Authentication Pages
- **Login** (`/auth/login`) - User sign in
- **Register** (`/auth/register`) - Create new account
- **Forgot Password** (`/auth/forgot-password`) - Password reset
- **Email Confirmation** (`/auth/confirm-email`) - Verify email

---

## 🤝 Contributing

### Development Guidelines
1. **Code Style**: Follow TypeScript and React best practices
2. **Component Structure**: Use functional components with hooks
3. **State Management**: Leverage React's built-in state and server actions
4. **Styling**: Follow Bootstrap conventions and custom CSS guidelines
5. **Type Safety**: Maintain strict TypeScript typing

### File Naming Conventions
- **Components**: PascalCase (e.g., `ProductCard.tsx`)
- **Pages**: kebab-case (e.g., `product-details`)
- **Utilities**: camelCase (e.g., `formatCurrency.ts`)
- **Types**: PascalCase (e.g., `ProductDto.ts`)

---

## 📄 License

This project is part of the BlueBerry24 e-commerce platform. See the main project repository for license information.

---

## 🔗 Related Projects

- **[BlueBerry24.API](../BlueBerry24.API/)** - Backend REST API
- **[BlueBerry24.Application](../BlueBerry24.Application/)** - Business logic layer
- **[BlueBerry24.Domain](../BlueBerry24.Domain/)** - Domain entities and rules
- **[BlueBerry24.Infrastructure](../BlueBerry24.Infrastructure/)** - Data access layer

---

## 📞 Support

For questions, issues, or contributions:

1. **Issues**: Create an issue in the main repository
2. **Documentation**: Check the main project README
3. **API Reference**: Review the backend API documentation

---

**Built with ❤️ using Next.js 15 and modern web technologies**
