using BlueBerry24.Domain.Constants;
using BlueBerry24.Domain.Entities.AuthEntities;
using BlueBerry24.Domain.Entities.CouponEntities;
using BlueBerry24.Domain.Entities.ProductEntities;
using BlueBerry24.Domain.Entities.ShopEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BlueBerry24.Infrastructure.Data
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IConfiguration _configuration;

        public DataSeeder(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IHostEnvironment hostEnvironment,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _hostEnvironment = hostEnvironment;
            _configuration = configuration;
        }

        public async Task SeedDataAsync()
        {
            var runSeedOnStartup = _configuration.GetValue("Database:RunSeedOnStartup", false);
            if (!_hostEnvironment.IsDevelopment() && !runSeedOnStartup)
            {
                Console.WriteLine("Seeder skipped: use Development environment or set Database:RunSeedOnStartup=true.");
                return;
            }

            try
            {
                Console.WriteLine("Starting database seeding...");

                await SeedRolesAsync();
                await SeedUsersAsync();

                await SeedShopAsync();
                await _context.SaveChangesAsync();

                await SeedCategoriesAsync();
                await _context.SaveChangesAsync();

                await SeedProductsAsync();
                await _context.SaveChangesAsync();

                await SeedCouponsAsync();
                await _context.SaveChangesAsync();

                Console.WriteLine("Database seeding completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during database seeding: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        private async Task SeedRolesAsync()
        {
            Console.WriteLine("Seeding roles...");

            var roles = new[]
            {
                RoleConstants.SuperAdmin,
                RoleConstants.Admin,
                RoleConstants.User
            };

            foreach (var roleName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var role = new ApplicationRole(roleName);
                    await _roleManager.CreateAsync(role);
                    Console.WriteLine($"Created role: {roleName}");
                }
            }
        }

        private async Task SeedUsersAsync()
        {
            Console.WriteLine("Seeding users...");

            var superAdminPass = Environment.GetEnvironmentVariable("SEED_SUPERADMIN_PASSWORD");
            if (string.IsNullOrWhiteSpace(superAdminPass))
            {
                superAdminPass = "SuperAdmin@123!";
            }

            await CreateUserAsync(
                email: "shahade.abdulhamid@gmail.com",
                firstName: "Abdulhamid",
                lastName: "Shahade",
                role: RoleConstants.SuperAdmin,
                password: superAdminPass
            );
        }

        private async Task CreateUserAsync(string email, string firstName, string lastName, string role, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine($"Skipped creating user {email}: password not configured.");
                return;
            }

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role);
                    Console.WriteLine($"Created user: {email} with role: {role}");
                }
                else
                {
                    Console.WriteLine($"Failed to create user {email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }

        private async Task SeedShopAsync()
        {
            Console.WriteLine("Seeding shop configuration...");

            if (!await _context.Shops.AnyAsync())
            {
                var shop = new Shop
                {
                    Name = "BlueBerry24 Store",
                    Description = "Your premium online shopping destination for quality products",
                    Email = "info@blueberry24.com",
                    Phone = "+111111111111",
                    Address = "Berlin",
                    Currency = "TR",
                    Language = "English",
                    LogoUrl = "BlueBerry24 Sample Logo"
                };

                await _context.Shops.AddAsync(shop);
                Console.WriteLine("Created shop configuration");
            }
        }

        private async Task SeedCategoriesAsync()
        {
            Console.WriteLine("Seeding categories...");

            if (!await _context.Categories.AnyAsync())
            {
                var categories = new[]
                {
                    new Category
                    {
                        Name = "Vegetables & Fruits",
                        Description = "Fresh, handpicked vegetables and fruits delivered to your doorstep. Shop seasonal produce and daily essentials for a healthier lifestyle.",
                        ImageUrl = "/uploads/category/1749147642485_category_bf91ecc8-6812-4b6f-9c1c-69daf759c4e6.png",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Category
                    {
                        Name = "Water & Drinks",
                        Description = "Stay refreshed with our wide selection of bottled water, juices, soft drinks, and health beverages. Perfect for every occasion and thirst.",
                        ImageUrl = "/uploads/category/1749040341033_category_d45f9697-2f0d-4829-b3d3-8951acaa25f1.png",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Category
                    {
                        Name = "Snack",
                        Description = "Tasty treats and crunchy delights for every craving â€” explore chips, cookies, nuts, and more for your perfect snack break.",
                        ImageUrl = "/uploads/category/1749040352113_category_f8b48944-5006-4f27-a3e4-8f46017040ab.png",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Category
                    {
                        Name = "Breakfast",
                        Description = "Start your day right with our breakfast essentials â€” cereals, spreads, oats, and more to fuel your morning routine.",
                        ImageUrl = "/uploads/category/1749040358773_category_b4bd18bc-0823-4197-a654-886c2834e0a0.png",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                _context.Categories.AddRange(categories);
                Console.WriteLine($"Created {categories.Length} categories");
            }
        }

        private async Task SeedProductsAsync()
        {
            Console.WriteLine("Seeding products...");

            if (!await _context.Products.AnyAsync())
            {
                try
                {
                    // Get categories for product assignment with null checks
                    var vegetablesFruits = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Vegetables & Fruits");
                    var waterDrinks = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Water & Drinks");
                    var snack = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Snack");
                    var breakfast = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Breakfast");

                    if (vegetablesFruits == null || waterDrinks == null || snack == null || breakfast == null)
                    {
                        Console.WriteLine("Categories not found. Skipping product seeding.");
                        return;
                    }

                    var products = new[]
                    {
                new Product
{
    Name = "Organic Gala Apples",
    Description = "Crisp, sweet, and organically grown Gala apples.",
    Price = 2.99m,
    StockQuantity = 149,
    ImageUrl = "/uploads/product/1749037220209_product_b13196f5-1d26-4d74-9b12-2a6e03696108.png",
    SKU = "VF-001",
    IsActive = true,
    LowStockThreshold = 20,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 116,
},
new Product
{
    Name = "Fresh Broccoli Florets",
    Description = "Pre-cut fresh broccoli florets, ready to cook.",
    Price = 3.49m,
    StockQuantity = 100,
    ImageUrl = "/uploads/product/1749037264045_product_b53023e1-135b-443d-9019-adcd36f823f5.png",
    SKU = "VF-002",
    IsActive = true,
    LowStockThreshold = 15,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 10,
},
new Product
{
    Name = "Ripe Avocados (Hass)",
    Description = "Creamy and ripe Hass avocados, perfect for guacamole.",
    Price = 1.79m,
    StockQuantity = 75,
    ImageUrl = "/uploads/product/1749039587526_product_19a502da-cea0-487d-9b8a-6cc32bdd7416.png",
    SKU = "VF-003",
    IsActive = true,
    LowStockThreshold = 10,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 7,
},
new Product
{
    Name = "Sweet Bell Peppers (Mixed)",
    Description = "Assorted red, yellow, and green bell peppers.",
    Price = 4.29m,
    StockQuantity = 70,
    ImageUrl = "/uploads/product/1749041709806_product_ddc0652a-4ceb-44eb-bfbc-1e43e52b4ed2.png",
    SKU = "VF-004",
    IsActive = true,
    LowStockThreshold = 12,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 8,
},
new Product
{
    Name = "Spring Water (1L Bottle)",
    Description = "Pure natural spring water in a convenient 1-liter bottle.",
    Price = 0.99m,
    StockQuantity = 500,
    ImageUrl = "/uploads/product/1749116029668_product_3d7f9893-f1ab-4f86-bbf2-303402e09fd7.png",
    SKU = "WD-001",
    IsActive = true,
    LowStockThreshold = 75,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 50,
},
new Product
{
    Name = "Sparkling Water (Cans, 6-Pack)",
    Description = "Refreshing sparkling water, zero calories, 6-pack.",
    Price = 3.49m,
    StockQuantity = 300,
    ImageUrl = "/uploads/product/1749115929567_product_7a0717ee-a07f-41e0-bc80-833425e08baf.png",
    SKU = "WD-002",
    IsActive = true,
    LowStockThreshold = 45,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 30,
},
new Product
{
    Name = "Orange Juice (Not from Concentrate, 1.5L)",
    Description = "100% pure orange juice, not from concentrate, 1.5 liters.",
    Price = 4.79m,
    StockQuantity = 150,
    ImageUrl = "/uploads/product/1749116076087_product_38c5fe91-17e9-468c-be82-99e7376c90a8.png",
    SKU = "WD-003",
    IsActive = true,
    LowStockThreshold = 20,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 15,
},
new Product
{
    Name = "Cola Soda (12-Pack Cans)",
    Description = "Classic cola soda, 12-pack of cans.",
    Price = 6.99m,
    StockQuantity = 250,
    ImageUrl = "/uploads/product/1749116137138_product_8ecf5e43-0775-4072-8590-2e7dc1d3dc77.png",
    SKU = "WD-004",
    IsActive = true,
    LowStockThreshold = 35,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 25,
},
new Product
{
    Name = "Potato Chips (Classic Salted, Large Bag)",
    Description = "Crispy potato chips, perfectly salted.",
    Price = 3.49m,
    StockQuantity = 300,
    ImageUrl = "/uploads/product/1749116204662_product_bd31b9b2-a35d-42bd-a0e6-ba01dc542549.png",
    SKU = "SN-001",
    IsActive = true,
    LowStockThreshold = 45,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 30,
},
new Product
{
    Name = "Pretzel Twists (Family Size Bag)",
    Description = "Traditional pretzel twists, great for sharing.",
    Price = 2.99m,
    StockQuantity = 250,
    ImageUrl = "/uploads/product/1749116352273_product_5698c433-afff-4d46-ae52-31a960e62def.png",
    SKU = "SN-002",
    IsActive = true,
    LowStockThreshold = 35,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 25,
},
new Product
{
    Name = "Mixed Nuts (Unsalted, 16oz)",
    Description = "A healthy mix of unsalted almonds, cashews, and peanuts.",
    Price = 8.99m,
    StockQuantity = 150,
    ImageUrl = "/uploads/product/1749118085100_product_a592235f-b059-4375-9216-c51bf1fb5239.png",
    SKU = "SN-003",
    IsActive = true,
    LowStockThreshold = 20,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 15,
},
new Product
{
    Name = "Granola Bars (Variety Pack, 10-Count)",
    Description = "Assorted flavors of wholesome granola bars.",
    Price = 5.79m,
    StockQuantity = 200,
    ImageUrl = "/uploads/product/1749118126120_product_4a46dc92-5b32-4f4e-8627-2b05063107ed.png",
    SKU = "SN-004",
    IsActive = true,
    LowStockThreshold = 30,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 20,
},
new Product
{
    Name = "Rolled Oats (Large Canister)",
    Description = "Hearty rolled oats for a warm and nutritious breakfast.",
    Price = 4.99m,
    StockQuantity = 200,
    ImageUrl = "/uploads/product/1749124777000_product_42437042-a874-47af-b535-b868901076a9.png",
    SKU = "BF-001",
    IsActive = true,
    LowStockThreshold = 30,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 20,
},
new Product
{
    Name = "Maple Syrup (Pure, Grade A, 16oz)",
    Description = "100% pure maple syrup, perfect for pancakes.",
    Price = 12.99m,
    StockQuantity = 120,
    ImageUrl = "/uploads/product/1749124807016_product_caf98b12-95fa-4e42-b39f-6461a38faf78.png",
    SKU = "BF-002",
    IsActive = true,
    LowStockThreshold = 18,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 12,
},
new Product
{
    Name = "Pancake Mix (Buttermilk, Large Box)",
    Description = "Easy-to-prepare buttermilk pancake mix.",
    Price = 3.79m,
    StockQuantity = 150,
    ImageUrl = "/uploads/product/1749124842275_product_d23437c6-5b91-4bff-965f-9d082af74f7e.png",
    SKU = "BF-003",
    IsActive = true,
    LowStockThreshold = 20,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 15,
},
new Product
{
    Name = "Assorted Cereal (Family Size Box)",
    Description = "A popular family-size box of assorted breakfast cereal.",
    Price = 6.49m,
    StockQuantity = 248,
    ImageUrl = "/uploads/product/1749124878284_product_920a8398-44ac-4662-97db-11e1a709527a.png",
    SKU = "BF-004",
    IsActive = true,
    LowStockThreshold = 35,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 31,
},
            };

                    await _context.Products.AddRangeAsync(products);
                    await _context.SaveChangesAsync();

                    Console.WriteLine($"Created {products.Length} products");

                    var productCategories = new List<ProductCategory>();
                    var utcNow = DateTime.UtcNow;
                    void LinkSlice(int startIndex, int count, Category category)
                    {
                        for (var i = 0; i < count; i++)
                        {
                            productCategories.Add(new ProductCategory
                            {
                                ProductId = products[startIndex + i].Id,
                                CategoryId = category.Id,
                                CreatedAt = utcNow,
                                UpdatedAt = utcNow
                            });
                        }
                    }

                    LinkSlice(0, 4, vegetablesFruits);
                    LinkSlice(4, 4, waterDrinks);
                    LinkSlice(8, 4, snack);
                    LinkSlice(12, 4, breakfast);

                    await _context.ProductCategories.AddRangeAsync(productCategories);
                    await _context.SaveChangesAsync();

                    Console.WriteLine($"Created product-category relationships");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error seeding products: {ex.Message}");
                    throw;
                }
            }
        }

        private async Task SeedCouponsAsync()
        {
            Console.WriteLine("Seeding coupons...");

            if (!await _context.Coupons.AnyAsync())
            {
                var seedTime = new DateTime(2026, 2, 11, 23, 7, 3, 498, DateTimeKind.Utc);

                var coupons = new[]
                {
                    new Coupon
                    {
                        Code = "JUNESALE20",
                        Description = "20% off all orders over $75",
                        Type = CouponType.Percentage,
                        Value = 0.20m,
                        DiscountAmount = 20.00m,
                        MinimumOrderAmount = 75.00m,
                        IsActive = true,
                        IsForNewUsersOnly = true,
                        CreatedAt = seedTime,
                        UpdatedAt = seedTime,
                    },
                    new Coupon
                    {
                        Code = "SAVE15OFF",
                        Description = "Get $15 off your next purchase over $100",
                        Type = CouponType.FixedAmount,
                        Value = 15.00m,
                        DiscountAmount = 15.00m,
                        MinimumOrderAmount = 100.00m,
                        IsActive = true,
                        IsForNewUsersOnly = false,
                        CreatedAt = seedTime,
                        UpdatedAt = seedTime,
                    },
                    new Coupon
                    {
                        Code = "WKNDFLASH",
                        Description = "10% off on electronics this weekend on orders over $75",
                        Type = CouponType.Percentage,
                        Value = 0.10m,
                        DiscountAmount = 10.00m,
                        MinimumOrderAmount = 75.00m,
                        IsActive = true,
                        IsForNewUsersOnly = false,
                        CreatedAt = seedTime,
                        UpdatedAt = seedTime,
                    },
                    new Coupon
                    {
                        Code = "APRILDEAL",
                        Description = "Flat $50 off on orders over $250",
                        Type = CouponType.FixedAmount,
                        Value = 50.00m,
                        DiscountAmount = 50.00m,
                        MinimumOrderAmount = 250.00m,
                        IsActive = true,
                        IsForNewUsersOnly = false,
                        CreatedAt = seedTime,
                        UpdatedAt = seedTime,
                    },
                    new Coupon
                    {
                        Code = "ECOFRIENDLY",
                        Description = "12% off sustainable products",
                        Type = CouponType.Percentage,
                        Value = 0.12m,
                        DiscountAmount = 12.00m,
                        MinimumOrderAmount = 50.00m,
                        IsActive = true,
                        IsForNewUsersOnly = false,
                        CreatedAt = seedTime,
                        UpdatedAt = seedTime,
                    },
                    new Coupon
                    {
                        Code = "LOYALTYDISC",
                        Description = "Special 5% discount for returning customers",
                        Type = CouponType.Percentage,
                        Value = 0.05m,
                        DiscountAmount = 5.00m,
                        MinimumOrderAmount = 50.00m,
                        IsActive = true,
                        IsForNewUsersOnly = false,
                        CreatedAt = seedTime,
                        UpdatedAt = seedTime,
                    },
                    new Coupon
                    {
                        Code = "HOLIDAY2025",
                        Description = "Flat $10 off any purchase",
                        Type = CouponType.FixedAmount,
                        Value = 10.00m,
                        DiscountAmount = 10.00m,
                        MinimumOrderAmount = 0.00m,
                        IsActive = true,
                        IsForNewUsersOnly = false,
                        CreatedAt = seedTime,
                        UpdatedAt = seedTime,
                    },
                    new Coupon
                    {
                        Code = "NEWBIE25",
                        Description = "25% off your very first order",
                        Type = CouponType.Percentage,
                        Value = 0.25m,
                        DiscountAmount = 25.00m,
                        MinimumOrderAmount = 50.00m,
                        IsActive = true,
                        IsForNewUsersOnly = true,
                        CreatedAt = seedTime,
                        UpdatedAt = seedTime,
                    },
                    new Coupon
                    {
                        Code = "WELCOMEBACK",
                        Description = "Get $20 off your initial order over $75",
                        Type = CouponType.FixedAmount,
                        Value = 20.00m,
                        DiscountAmount = 20.00m,
                        MinimumOrderAmount = 75.00m,
                        IsActive = true,
                        IsForNewUsersOnly = true,
                        CreatedAt = seedTime,
                        UpdatedAt = seedTime,
                    },
                };

                _context.Coupons.AddRange(coupons);
                Console.WriteLine($"Created {coupons.Length} coupons");
            }
        }
    }
}
