# BlueBerry24

A modular, **.NET 9** e-commerce back-end built with Clean Architecture principles.  
The solution is split into four projects:

| Project | Responsibility |
|---------|----------------|
| **BlueBerry24.Domain**        | Enterprise rules, entities, value objects & validations. No external dependencies except for cross-cutting libraries (e.g., FluentValidation, Identity, Redis). |
| **BlueBerry24.Application**   | Application logic (DTOs, services, AutoMapper profiles).  Depends only on *Domain*. |
| **BlueBerry24.Infrastructure**| External concerns: Entity Framework Core, SQL Server persistence, Identity stores, Redis cache, JSON utilities, repository implementations.  Depends on *Domain*. |
| **BlueBerry24.API**           | Thin ASP.NET REST interface (Minimal API style) that wires the layers together and exposes OpenAPI metadata. |

---

## ✨ Features

* Advanced endpoints for Products, Shops, Stock, Shopping Carts, Coupons and Auth.
* FluentValidation-based model validation.
* Entity Framework Core with SQL Server provider.
* ASP.NET Identity (custom `ApplicationUser` / `ApplicationRole`).
* Redis caching layer.
* AutoMapper mapping between entities and DTOs.
* OpenAPI spec generation for Swagger / CLI clients.

---

## 🗂 Project Structure (Simplified)
```text
BlueBerry24/
├── BlueBerry24.API/                # HTTP entry-point
│   └── Program.cs
├── BlueBerry24.Application/        # Use-cases & DTOs
│   ├── Mapping/
│   └── Services/
├── BlueBerry24.Domain/             # Core domain model
│   └── Entities/
└── BlueBerry24.Infrastructure/     # Persistence & integrations
    └── Data/
```
---

## 🛠 Tech Stack

* .NET 9 (preview)
* ASP.NET Core 9
* Entity Framework Core 9 + SQL Server
* FluentValidation 11
* AutoMapper
* Redis (StackExchange.Redis)
* Swashbuckle / Microsoft.AspNetCore.OpenApi

---

## ⚡️ Quick Start

1. **Prerequisites**
   * [.NET 9 SDK](https://dotnet.microsoft.com/) (Preview Channel)  
     `dotnet --version` ➜ `9.*`
   * SQL Server (localdb or full instance)
   * Optional: Redis if you want to test caching locally.

2. **Clone & restore**

   ```bash
   git clone https://github.com/your-org/BlueBerry24.git
   cd BlueBerry24
   dotnet restore
   ```

3. **Configure the connection string**

   The Infrastructure layer expects a `DefaultConnection` string.  
   You can set it in **`BlueBerry24.API/appsettings.Development.json`** or via **user-secrets**:

   ```bash
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.;Database=BlueBerry24;Trusted_Connection=True;MultipleActiveResultSets=true"
   ```

4. **Run database migrations**

   ```bash
   dotnet ef database update --startup-project BlueBerry24.API --project BlueBerry24.Infrastructure
   ```

5. **Run the API**

   ```bash
   dotnet run --project BlueBerry24.API
   ```

   The API starts on `https://localhost:5001`.  
   Browse to `/swagger` to explore the endpoints.

---

## 🧪 Testing

Unit and integration tests are planned but not yet committed.  
When added they will live in a dedicated `BlueBerry24.Tests` project.

---

## 🛡️ Environment Configuration

| Setting | Description |
|---------|-------------|
| `ConnectionStrings:DefaultConnection` | SQL Server connection string. |
| `Redis:ConnectionString`              | Redis endpoint (`host:port`). |
| `Jwt:Key`, `Jwt:Issuer`, …            | JWT options for Auth. |

Use `appsettings.{Environment}.json` or environment variables for secrets in production.

---

## 🤝 Contributing

1. Fork the repo & create your feature branch: `git checkout -b feature/awesome-thing`
2. Commit your changes: `git commit -m "feat: add awesome thing"`
3. Push to the branch: `git push origin feature/awesome-thing`
4. Open a pull request.

Please follow the existing project layout and coding style (nullable enabled, implicit usings etc.).

---

## 📄 License

Distributed under the MIT License.  
See `LICENSE` for more information.
