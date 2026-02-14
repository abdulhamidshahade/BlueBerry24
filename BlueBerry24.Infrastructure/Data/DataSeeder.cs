using BlueBerry24.Domain.Constants;
using BlueBerry24.Domain.Entities.AuthEntities;
using BlueBerry24.Domain.Entities.CouponEntities;
using BlueBerry24.Domain.Entities.ProductEntities;
using BlueBerry24.Domain.Entities.ShopEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry24.Infrastructure.Data
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public DataSeeder(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedDataAsync()
        {
            // ⚠️ SAFETY CHECK: Only allow seeding in Development environment
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (env != "Development")
            {
                Console.WriteLine("⚠️ Seeder aborted: Only allowed in Development environment");
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

            var superAdminPass = Environment.GetEnvironmentVariable("SEED_SUPERADMIN_PASSWORD") ?? "SuperAdmin@123!";
            var adminPass = Environment.GetEnvironmentVariable("SEED_ADMIN_PASSWORD") ?? "Admin@123!";
            var userPass = Environment.GetEnvironmentVariable("SEED_USER_PASSWORD") ?? "User@123!";

            await CreateUserAsync(
                email: "superadmin@blueberry24.com",
                firstName: "Super",
                lastName: "Admin",
                role: RoleConstants.SuperAdmin,
                password: superAdminPass
            );

            await CreateUserAsync(
                email: "admin@blueberry24.com",
                firstName: "Admin",
                lastName: "User",
                role: RoleConstants.Admin,
                password: adminPass
            );

            await CreateUserAsync(
                email: "user@blueberr24.com",
                firstName: "User",
                lastName: "1",
                role: RoleConstants.User,
                password: userPass
            );
        }

        private async Task CreateUserAsync(string email, string firstName, string lastName, string role, string password)
        {
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
                    Address = "Dulkadiroglu",
                    Currency = "TR",
                    Language = "Türkçe",
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
                        Description = "Tasty treats and crunchy delights for every craving — explore chips, cookies, nuts, and more for your perfect snack break.",
                        ImageUrl = "/uploads/category/1749040352113_category_f8b48944-5006-4f27-a3e4-8f46017040ab.png",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Category
                    {
                        Name = "Breakfast",
                        Description = "Start your day right with our breakfast essentials — cereals, spreads, oats, and more to fuel your morning routine.",
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
    Name = "Bananas (Organic)",
    Description = "Organically grown Cavendish bananas.",
    Price = 0.79m,
    StockQuantity = 197,
    ImageUrl = "/uploads/product/1749041829787_product_86584d50-5f7d-4f08-af97-343f41c3b2b2.png",
    SKU = "VF-005",
    IsActive = true,
    LowStockThreshold = 30,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 20,
},
new Product
{
    Name = "Baby Spinach Bag",
    Description = "Fresh baby spinach, pre-washed and ready-to-eat.",
    Price = 2.50m,
    StockQuantity = 119,
    ImageUrl = "/uploads/product/1749041908155_product_c5d1a3ec-8b37-432b-ac4e-8032cf71e654.png",
    SKU = "VF-006",
    IsActive = true,
    LowStockThreshold = 18,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 12,
},
new Product
{
    Name = "Blueberries (Fresh)",
    Description = "Sweet and juicy fresh blueberries.",
    Price = 3.99m,
    StockQuantity = 60,
    ImageUrl = "/uploads/product/1749041967930_product_b63b4500-ad7a-4c96-80ed-d9c348f0076c.png",
    SKU = "VF-007",
    IsActive = true,
    LowStockThreshold = 9,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 8,
},
new Product
{
    Name = "Carrots (Bunch)",
    Description = "A fresh bunch of garden carrots.",
    Price = 1.99m,
    StockQuantity = 90,
    ImageUrl = "/uploads/product/1749042231514_product_cf32d7a7-795c-447b-b2aa-2a0e453f67db.png",
    SKU = "VF-008",
    IsActive = true,
    LowStockThreshold = 13,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 10,
},
new Product
{
    Name = "Large Navel Oranges",
    Description = "Sweet and seedless Navel oranges.",
    Price = 1.29m,
    StockQuantity = 110,
    ImageUrl = "/uploads/product/1749042343515_product_1a5062c5-b905-45e5-a9c0-7f887dbe479a.png",
    SKU = "VF-009",
    IsActive = true,
    LowStockThreshold = 16,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 11,
},
new Product
{
    Name = "Crimini Mushrooms",
    Description = "Earthy crimini mushrooms, perfect for sautéing.",
    Price = 3.19m,
    StockQuantity = 50,
    ImageUrl = "/uploads/product/1749042420119_product_bbc804d0-9537-45af-84bb-b00a2d68fe45.png",
    SKU = "VF-010",
    IsActive = true,
    LowStockThreshold = 8,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 5,
},
new Product
{
    Name = "Red Seedless Grapes",
    Description = "Crisp and sweet red seedless grapes.",
    Price = 4.50m,
    StockQuantity = 70,
    ImageUrl = "/uploads/product/1749042490458_product_18332416-3157-4805-bbe5-fff7e1895cda.png",
    SKU = "VF-011",
    IsActive = true,
    LowStockThreshold = 10,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 7,
},
new Product
{
    Name = "Roma Tomatoes",
    Description = "Firm Roma tomatoes, ideal for sauces and salads.",
    Price = 2.29m,
    StockQuantity = 130,
    ImageUrl = "/uploads/product/1749042659314_product_94c142cf-157a-4b81-bd28-9b25a38fb97e.png",
    SKU = "VF-012",
    IsActive = true,
    LowStockThreshold = 19,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 13,
},
new Product
{
    Name = "Lemons (Organic)",
    Description = "Tart and fresh organic lemons.",
    Price = 0.89m,
    StockQuantity = 85,
    ImageUrl = "/uploads/product/1749042789049_product_78e33b27-5ef6-4239-9ea7-22e1cf7e9571.png",
    SKU = "VF-013",
    IsActive = true,
    LowStockThreshold = 12,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 8,
},
new Product
{
    Name = "Green Cabbage",
    Description = "Large, fresh green cabbage head.",
    Price = 1.49m,
    StockQuantity = 40,
    ImageUrl = "/uploads/product/1749042921632_product_994deb41-3253-4585-8902-b6d127196867.png",
    SKU = "VF-014",
    IsActive = true,
    LowStockThreshold = 6,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 4,
},
new Product
{
    Name = "Strawberries (1lb)",
    Description = "Sweet and juicy fresh strawberries.",
    Price = 4.99m,
    StockQuantity = 95,
    ImageUrl = "/uploads/product/1749044965653_product_d35bdea4-9907-4e19-a277-769e8dedd671.png",
    SKU = "VF-015",
    IsActive = true,
    LowStockThreshold = 14,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 9,
},
new Product
{
    Name = "Sweet Potatoes",
    Description = "Nutrient-rich sweet potatoes.",
    Price = 1.69m,
    StockQuantity = 105,
    ImageUrl = "/uploads/product/1749045057797_product_b50c23a4-8801-4ea5-90fc-e6579e9fc70a.png",
    SKU = "VF-016",
    IsActive = true,
    LowStockThreshold = 15,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 10,
},
new Product
{
    Name = "Cucumbers",
    Description = "Crisp slicing cucumbers.",
    Price = 0.99m,
    StockQuantity = 70,
    ImageUrl = "/uploads/product/1749045121746_product_736d334c-2788-45d7-8efc-ef9518d5d9f4.png",
    SKU = "VF-017",
    IsActive = true,
    LowStockThreshold = 10,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 7,
},
new Product
{
    Name = "Pears (Bartlett)",
    Description = "Sweet and juicy Bartlett pears.",
    Price = 2.79m,
    StockQuantity = 65,
    ImageUrl = "/uploads/product/1749045487977_product_e4917a8c-50e9-47a8-abd0-f938516d1f04.png",
    SKU = "VF-018",
    IsActive = true,
    LowStockThreshold = 9,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 6,
},
new Product
{
    Name = "Yellow Onions (Bag)",
    Description = "Versatile yellow onions, essential for cooking.",
    Price = 1.19m,
    StockQuantity = 140,
    ImageUrl = "/uploads/product/1749045911239_product_3126ae81-7ae0-4911-8bc5-51c2b7882b94.png",
    SKU = "VF-019",
    IsActive = true,
    LowStockThreshold = 20,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 14,
},
new Product
{
    Name = "Cherries (Seasonal)",
    Description = "Sweet and tart seasonal cherries.",
    Price = 5.99m,
    StockQuantity = 55,
    ImageUrl = "/uploads/product/1749056088932_product_41c7a803-c648-482c-aab6-a3b20c351184.png",
    SKU = "VF-020",
    IsActive = true,
    LowStockThreshold = 8,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 5,
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
    Name = "Diet Lemon-Lime Soda (2L Bottle)",
    Description = "Zero sugar lemon-lime flavored soda, 2-liter bottle.",
    Price = 2.29m,
    StockQuantity = 180,
    ImageUrl = "/uploads/product/1749124922021_product_d4b10cfa-5fc1-4fad-b740-e9d8865aff30.png",
    SKU = "WD-005",
    IsActive = true,
    LowStockThreshold = 25,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 18,
},
new Product
{
    Name = "Unsweetened Iced Tea (Gallon)",
    Description = "Freshly brewed unsweetened iced tea, gallon size.",
    Price = 3.99m,
    StockQuantity = 100,
    ImageUrl = "/uploads/product/1749124958070_product_3633b06d-a5d1-462c-8a6a-62c82a93b4a1.png",
    SKU = "WD-006",
    IsActive = true,
    LowStockThreshold = 15,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 10,
},
new Product
{
    Name = "Green Tea (Bottled, 500ml)",
    Description = "Ready-to-drink green tea, 500ml bottle.",
    Price = 1.50m,
    StockQuantity = 200,
    ImageUrl = "/uploads/product/1749124992641_product_f4db19d5-68d3-4541-8914-f3c2c1151a64.png",
    SKU = "WD-007",
    IsActive = true,
    LowStockThreshold = 30,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 20,
},
new Product
{
    Name = "Apple Juice (1L Carton)",
    Description = "Pure apple juice in a convenient 1-liter carton.",
    Price = 2.89m,
    StockQuantity = 148,
    ImageUrl = "/uploads/product/1749125017796_product_c197c8dc-9744-4e90-ab12-1c2077e08790.png",
    SKU = "WD-008",
    IsActive = true,
    LowStockThreshold = 22,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 25,
},
new Product
{
    Name = "Coconut Water (1L Carton)",
    Description = "Natural coconut water, hydrating and refreshing.",
    Price = 3.79m,
    StockQuantity = 90,
    ImageUrl = "/uploads/product/1749125059199_product_7a8448d3-cd35-49c5-9057-aa5a764f8e0e.png",
    SKU = "WD-009",
    IsActive = true,
    LowStockThreshold = 13,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 9,
},
new Product
{
    Name = "Energy Drink (Can, Single)",
    Description = "High-performance energy drink for a quick boost.",
    Price = 2.49m,
    StockQuantity = 120,
    ImageUrl = "/uploads/product/1749125091213_product_2501c879-abc9-4f7a-b174-93e0a0c77ea9.png",
    SKU = "WD-010",
    IsActive = true,
    LowStockThreshold = 18,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 12,
},
new Product
{
    Name = "Sports Drink (Blue Raspberry, 750ml)",
    Description = "Electrolyte-rich sports drink, blue raspberry flavor.",
    Price = 1.99m,
    StockQuantity = 130,
    ImageUrl = "/uploads/product/1749125162651_product_0b3cbd4c-12ce-4627-adea-20b1e528d0ed.png",
    SKU = "WD-011",
    IsActive = true,
    LowStockThreshold = 19,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 13,
},
new Product
{
    Name = "Almond Milk (Unsweetened, 1.89L)",
    Description = "Unsweetened almond milk, dairy-free alternative.",
    Price = 3.29m,
    StockQuantity = 110,
    ImageUrl = "/uploads/product/1749125196965_product_ca1ed250-60db-424d-b5c5-abbb1b6a4051.png",
    SKU = "WD-012",
    IsActive = true,
    LowStockThreshold = 16,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 14,
},
new Product
{
    Name = "Soy Milk (Original, 1L)",
    Description = "Original flavor soy milk, plant-based protein.",
    Price = 2.69m,
    StockQuantity = 80,
    ImageUrl = "/uploads/product/1749125245151_product_f72831dd-02c7-4180-9e17-325c4c34c656.png",
    SKU = "WD-013",
    IsActive = true,
    LowStockThreshold = 12,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 8,
},
new Product
{
    Name = "Tomato Juice (946ml Bottle)",
    Description = "Rich and savory tomato juice.",
    Price = 2.19m,
    StockQuantity = 70,
    ImageUrl = "/uploads/product/1749125276289_product_1ebe8dfa-a392-4705-b74b-349053b370da.png",
    SKU = "WD-014",
    IsActive = true,
    LowStockThreshold = 10,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 7,
},
new Product
{
    Name = "Cranberry Juice Cocktail (1.89L)",
    Description = "Sweet and tart cranberry juice cocktail.",
    Price = 3.59m,
    StockQuantity = 100,
    ImageUrl = "/uploads/product/1749125325484_product_726e1729-958d-4cd8-a6f9-ddb1be5ff38c.png",
    SKU = "WD-015",
    IsActive = true,
    LowStockThreshold = 15,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 10,
},
new Product
{
    Name = "Ginger Ale (6-Pack Cans)",
    Description = "Crisp and refreshing ginger ale soda.",
    Price = 4.19m,
    StockQuantity = 140,
    ImageUrl = "/uploads/product/1749125386352_product_330a5cdc-2027-4fec-8f01-fd89f6412942.png",
    SKU = "WD-016",
    IsActive = true,
    LowStockThreshold = 20,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 14,
},
new Product
{
    Name = "Lemonade (Classic, 1.5L)",
    Description = "Sweet and tangy classic lemonade.",
    Price = 2.99m,
    StockQuantity = 95,
    ImageUrl = "/uploads/product/1749125414801_product_fbec470f-c148-4b98-a844-5fc6597e6cb1.png",
    SKU = "WD-017",
    IsActive = true,
    LowStockThreshold = 14,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 9,
},
new Product
{
    Name = "Mineral Water (Glass Bottle, 750ml)",
    Description = "Premium sparkling mineral water in a glass bottle.",
    Price = 1.79m,
    StockQuantity = 60,
    ImageUrl = "/uploads/product/1749125450234_product_8351cf67-2e2e-4fc7-a3f2-fe8c6952719c.png",
    SKU = "WD-018",
    IsActive = true,
    LowStockThreshold = 9,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 6,
},
new Product
{
    Name = "Carbonated Fruit Drink (Strawberry, Can)",
    Description = "Fruity and bubbly strawberry-flavored carbonated drink.",
    Price = 1.29m,
    StockQuantity = 170,
    ImageUrl = "/uploads/product/1749125494812_product_ce34ace5-a115-42fa-b878-44a60f7437ad.png",
    SKU = "WD-019",
    IsActive = true,
    LowStockThreshold = 24,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 18,
},
new Product
{
    Name = "Distilled Water (1 Gallon)",
    Description = "Purified distilled water, ideal for appliances.",
    Price = 1.59m,
    StockQuantity = 220,
    ImageUrl = "/uploads/product/1749125541602_product_a6954f44-e31d-48c8-9252-75f3f8db4de5.png",
    SKU = "WD-020",
    IsActive = true,
    LowStockThreshold = 33,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 22,
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
    Name = "Cheese Crackers (Large Box)",
    Description = "Cheesy and crunchy crackers, perfect for snacking.",
    Price = 4.29m,
    StockQuantity = 179,
    ImageUrl = "/uploads/product/1749118171195_product_5231c828-6274-400b-ba05-c3d3880181d7.png",
    SKU = "SN-005",
    IsActive = true,
    LowStockThreshold = 25,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 18,
},
new Product
{
    Name = "Fruit Snacks (Mixed Berry, 12-Count)",
    Description = "Chewy fruit-flavored snacks with real fruit juice.",
    Price = 4.50m,
    StockQuantity = 220,
    ImageUrl = "/uploads/product/1749122631219_product_9a5ca33b-a64f-43fc-8175-b7c46da2bcba.png",
    SKU = "SN-006",
    IsActive = true,
    LowStockThreshold = 33,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 22,
},
new Product
{
    Name = "Popcorn (Microwave, Butter Flavor, 6-Pack)",
    Description = "Convenient microwave popcorn with rich butter flavor.",
    Price = 4.99m,
    StockQuantity = 280,
    ImageUrl = "/uploads/product/1749122652543_product_d0b39299-d90f-48bc-8b83-4bcdb0fc3fbf.png",
    SKU = "SN-007",
    IsActive = true,
    LowStockThreshold = 40,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 28,
},
new Product
{
    Name = "Beef Jerky (Original Flavor, 3oz)",
    Description = "Savory and high-protein beef jerky.",
    Price = 6.99m,
    StockQuantity = 100,
    ImageUrl = "/uploads/product/1749122664994_product_71963de6-b818-4e69-8d00-a366ecc8d7b1.png",
    SKU = "SN-008",
    IsActive = true,
    LowStockThreshold = 15,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 10,
},
new Product
{
    Name = "Rice Cakes (Lightly Salted)",
    Description = "Crisp and light rice cakes, lightly salted.",
    Price = 2.19m,
    StockQuantity = 120,
    ImageUrl = "/uploads/product/1749122681079_product_0a549ee6-c874-425f-9d52-d66f88579499.png",
    SKU = "SN-009",
    IsActive = true,
    LowStockThreshold = 18,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 12,
},
new Product
{
    Name = "Chocolate Chip Cookies (Family Pack)",
    Description = "Classic chocolate chip cookies, soft and chewy.",
    Price = 3.79m,
    StockQuantity = 160,
    ImageUrl = "/uploads/product/1749122692017_product_0952640f-92ab-4872-b6d7-b17ca732b466.png",
    SKU = "SN-010",
    IsActive = true,
    LowStockThreshold = 22,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 16,
},
new Product
{
    Name = "Trail Mix (Sweet & Salty, 10oz)",
    Description = "A balanced blend of nuts, seeds, and dried fruit.",
    Price = 5.49m,
    StockQuantity = 110,
    ImageUrl = "/uploads/product/1749122709692_product_59470225-58d2-432a-a824-f4be73447d94.png",
    SKU = "SN-011",
    IsActive = true,
    LowStockThreshold = 16,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 11,
},
new Product
{
    Name = "Veggie Straws (Sea Salt, Large Bag)",
    Description = "Crunchy veggie straws with a hint of sea salt.",
    Price = 3.99m,
    StockQuantity = 190,
    ImageUrl = "/uploads/product/1749122719671_product_ca707f03-eea8-410c-9d29-c7326ffcaba2.png",
    SKU = "SN-012",
    IsActive = true,
    LowStockThreshold = 28,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 19,
},
new Product
{
    Name = "Protein Bars (Chocolate Peanut Butter, 8-Count)",
    Description = "High-protein bars for an energy boost.",
    Price = 9.99m,
    StockQuantity = 140,
    ImageUrl = "/uploads/product/1749122874736_product_7819d024-1ce3-46b6-8832-c3b6cafaea41.png",
    SKU = "SN-013",
    IsActive = true,
    LowStockThreshold = 20,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 14,
},
new Product
{
    Name = "Oatmeal Cream Pies (12-Count)",
    Description = "Soft oatmeal cookies with a creamy filling.",
    Price = 3.29m,
    StockQuantity = 90,
    ImageUrl = "/uploads/product/1749122923398_product_cedb4d31-fa99-4ec9-965e-2a1c92899b1e.png",
    SKU = "SN-014",
    IsActive = true,
    LowStockThreshold = 13,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 9,
},
new Product
{
    Name = "Dried Apricots (Sweetened, 1lb)",
    Description = "Naturally sweet dried apricots, rich in fiber.",
    Price = 7.50m,
    StockQuantity = 80,
    ImageUrl = "/uploads/product/1749122969486_product_1b36c0f0-e540-4462-8d34-c8f611cf4dcf.png",
    SKU = "SN-015",
    IsActive = true,
    LowStockThreshold = 12,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 8,
},
new Product
{
    Name = "Animal Crackers (Large Box)",
    Description = "Fun animal-shaped crackers, great for kids.",
    Price = 2.69m,
    StockQuantity = 201,
    ImageUrl = "/uploads/product/1749123021735_product_b2769d2d-1edb-4842-a202-c153aeff8b78.png",
    SKU = "SN-016",
    IsActive = true,
    LowStockThreshold = 30,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 23,
},
new Product
{
    Name = "Roasted Seaweed Snacks (Original, 6-Pack)",
    Description = "Crispy and savory roasted seaweed sheets.",
    Price = 4.19m,
    StockQuantity = 70,
    ImageUrl = "/uploads/product/1749123471204_product_02c9d6d1-6607-4479-a2b6-00c493402cd4.png",
    SKU = "SN-017",
    IsActive = true,
    LowStockThreshold = 10,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 7,
},
new Product
{
    Name = "Ginger Snaps (Crispy Cookies)",
    Description = "Spicy and crisp ginger snap cookies.",
    Price = 3.19m,
    StockQuantity = 130,
    ImageUrl = "/uploads/product/1749123483486_product_011e7409-de98-40ca-a818-2678f3521426.png",
    SKU = "SN-018",
    IsActive = true,
    LowStockThreshold = 19,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 13,
},
new Product
{
    Name = "Pistachios (Roasted & Salted, 8oz)",
    Description = "Delicious roasted and salted pistachios in shell.",
    Price = 10.50m,
    StockQuantity = 60,
    ImageUrl = "/uploads/product/1749123493548_product_a597fcda-be78-4923-9cb7-71f51985e049.png",
    SKU = "SN-019",
    IsActive = true,
    LowStockThreshold = 9,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 6,
},
new Product
{
    Name = "Fig Bars (Whole Wheat, 12-Count)",
    Description = "Soft and chewy fig bars made with whole wheat.",
    Price = 4.89m,
    StockQuantity = 170,
    ImageUrl = "/uploads/product/1749123502371_product_1e1fb95e-a9c0-4139-be93-d001fe73324e.png",
    SKU = "SN-020",
    IsActive = true,
    LowStockThreshold = 24,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 17,
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
new Product
{
    Name = "Frozen Waffles (Homestyle, 10-Count)",
    Description = "Quick and easy homestyle frozen waffles.",
    Price = 3.99m,
    StockQuantity = 180,
    ImageUrl = "/uploads/product/1749124565618_product_9b24ed51-ac38-4d97-948c-3aa9308ae254.png",
    SKU = "BF-005",
    IsActive = true,
    LowStockThreshold = 25,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 18,
},
new Product
{
    Name = "Breakfast Sausage Links (Frozen, 12oz)",
    Description = "Savory frozen breakfast sausage links.",
    Price = 5.29m,
    StockQuantity = 99,
    ImageUrl = "/uploads/product/1749124643196_product_3b346d47-1152-4043-9ccc-9ac99d56f868.png",
    SKU = "BF-006",
    IsActive = true,
    LowStockThreshold = 15,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 14,
},
new Product
{
    Name = "Instant Coffee (Classic Roast, 7oz)",
    Description = "Convenient instant coffee for a quick morning brew.",
    Price = 7.89m,
    StockQuantity = 130,
    ImageUrl = "/uploads/product/1749124700846_product_a778009e-9f35-49ca-8865-ff972eda5e96.png",
    SKU = "BF-007",
    IsActive = true,
    LowStockThreshold = 19,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 13,
},
new Product
{
    Name = "Ground Coffee (Breakfast Blend, 12oz)",
    Description = "Smooth and balanced breakfast blend ground coffee.",
    Price = 9.49m,
    StockQuantity = 160,
    ImageUrl = "/uploads/product/1749124737029_product_cb82cdee-5373-4ab5-af02-b02d4b66ba37.png",
    SKU = "BF-008",
    IsActive = true,
    LowStockThreshold = 22,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 16,
},
new Product
{
    Name = "English Muffins (6-Count)",
    Description = "Toasted English muffins, perfect for eggs and butter.",
    Price = 2.50m,
    StockQuantity = 90,
    ImageUrl = "/uploads/product/1749124342310_product_8be8d00a-1a5f-4e21-b0aa-6c90ee6e004b.png",
    SKU = "BF-009",
    IsActive = true,
    LowStockThreshold = 13,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 9,
},
new Product
{
    Name = "Donut Holes (Powdered Sugar, 12oz Bag)",
    Description = "Sweet powdered sugar donut holes.",
    Price = 3.19m,
    StockQuantity = 110,
    ImageUrl = "/uploads/product/1749124386377_product_f324b261-625c-420d-980b-984e9701be1a.png",
    SKU = "BF-010",
    IsActive = true,
    LowStockThreshold = 16,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 13,
},
new Product
{
    Name = "Frozen Hash Browns (Shredded, 22oz)",
    Description = "Crispy shredded frozen hash browns.",
    Price = 2.79m,
    StockQuantity = 140,
    ImageUrl = "/uploads/product/1749124436931_product_c4fa3eab-d47f-4dc2-a60b-e8be7dec4f41.png",
    SKU = "BF-011",
    IsActive = true,
    LowStockThreshold = 20,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 14,
},
new Product
{
    Name = "Bagels (Plain, 6-Count)",
    Description = "Freshly baked plain bagels, ideal for breakfast.",
    Price = 3.49m,
    StockQuantity = 100,
    ImageUrl = "/uploads/product/1749124472537_product_5638f5d5-1ea3-4a74-917e-eca5024fdf91.png",
    SKU = "BF-012",
    IsActive = true,
    LowStockThreshold = 15,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 12,
},
new Product
{
    Name = "Strawberry Jam (Fruit Spread, 18oz)",
    Description = "Sweet strawberry jam, a classic breakfast spread.",
    Price = 4.19m,
    StockQuantity = 150,
    ImageUrl = "/uploads/product/1749124170157_product_475179b3-ebeb-40ad-ba89-71400ed7f87f.png",
    SKU = "BF-013",
    IsActive = true,
    LowStockThreshold = 20,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 15,
},
new Product
{
    Name = "Peanut Butter (Creamy, 16oz)",
    Description = "Smooth and creamy peanut butter.",
    Price = 3.89m,
    StockQuantity = 170,
    ImageUrl = "/uploads/product/1749124201257_product_9224e013-cfb4-4a5b-bb78-7f8d5d8bb279.png",
    SKU = "BF-014",
    IsActive = true,
    LowStockThreshold = 24,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 17,
},
new Product
{
    Name = "Whole Wheat Bread (Loaf)",
    Description = "Healthy whole wheat bread for toast.",
    Price = 2.99m,
    StockQuantity = 200,
    ImageUrl = "/uploads/product/1749124251991_product_cdb3d65e-c432-4746-b8be-45b7c3651d87.png",
    SKU = "BF-015",
    IsActive = true,
    LowStockThreshold = 30,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 20,
},
new Product
{
    Name = "Fruit & Grain Bars (Apple Cinnamon, 8-Count)",
    Description = "Wholesome fruit and grain bars for on-the-go breakfast.",
    Price = 4.69m,
    StockQuantity = 190,
    ImageUrl = "/uploads/product/1749124291608_product_00f7e65f-d3a9-42f0-8b93-7903aaaae772.png",
    SKU = "BF-016",
    IsActive = true,
    LowStockThreshold = 28,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 19,
},
new Product
{
    Name = "Eggs (Large, Dozen)",
    Description = "Farm-fresh large eggs, a breakfast staple.",
    Price = 3.29m,
    StockQuantity = 220,
    ImageUrl = "/uploads/product/1749123948409_product_580ae5df-bb24-4890-9ba6-fc997583d1b5.png",
    SKU = "BF-017",
    IsActive = true,
    LowStockThreshold = 33,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 22,
},
new Product
{
    Name = "Bacon (Smoked, 1lb Pack)",
    Description = "Crispy smoked bacon for a hearty breakfast.",
    Price = 7.99m,
    StockQuantity = 78,
    ImageUrl = "/uploads/product/1749123994233_product_1670e786-21c1-4f39-a889-8a3ddf9427aa.png",
    SKU = "BF-018",
    IsActive = true,
    LowStockThreshold = 12,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 8,
},
new Product
{
    Name = "Yogurt (Vanilla, 4-Pack Cups)",
    Description = "Creamy vanilla yogurt cups.",
    Price = 4.49m,
    StockQuantity = 160,
    ImageUrl = "/uploads/product/1749124040214_product_679ad344-b42d-4064-b9cb-36185064981c.png",
    SKU = "BF-019",
    IsActive = true,
    LowStockThreshold = 22,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 16,
},
new Product
{
    Name = "Grits (Quick-Cooking, 24oz)",
    Description = "Southern-style quick-cooking grits.",
    Price = 2.59m,
    StockQuantity = 95,
    ImageUrl = "/uploads/product/1749124124764_product_99af7c14-8074-489c-a526-05710606fc79.png",
    SKU = "BF-020",
    IsActive = true,
    LowStockThreshold = 14,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    ReservedStock = 9,
},
            };

                    await _context.Products.AddRangeAsync(products);
                    await _context.SaveChangesAsync();

                    Console.WriteLine($"Created {products.Length} products");

                    // Create product-category relationships after products are saved
                    var productCategories = new List<ProductCategory>
            {
                // Vegetables & Fruits
                new ProductCategory
{
    ProductId = 1,
    CategoryId = 1,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 2,
    CategoryId = 1,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 3,
    CategoryId = 1,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 4,
    CategoryId = 1,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 5,
    CategoryId = 1,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 6,
    CategoryId = 1,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 7,
    CategoryId = 1,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 8,
    CategoryId = 1,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 9,
    CategoryId = 1,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 10,
    CategoryId = 1,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 11,
    CategoryId = 1,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 12,
    CategoryId = 1,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 13,
    CategoryId = 1,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 14,
    CategoryId = 1,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 15,
    CategoryId = 1,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 16,
    CategoryId = 1,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 17,
    CategoryId = 1,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 18,
    CategoryId = 1,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 19,
    CategoryId = 1,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 20,
    CategoryId = 1,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 21,
    CategoryId = 2,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 22,
    CategoryId = 2,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 23,
    CategoryId = 2,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 24,
    CategoryId = 2,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 25,
    CategoryId = 3,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 26,
    CategoryId = 3,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 27,
    CategoryId = 3,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 28,
    CategoryId = 3,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 29,
    CategoryId = 3,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 30,
    CategoryId = 3,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 31,
    CategoryId = 3,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 32,
    CategoryId = 3,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 33,
    CategoryId = 3,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 34,
    CategoryId = 3,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 35,
    CategoryId = 3,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 36,
    CategoryId = 3,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 37,
    CategoryId = 3,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 38,
    CategoryId = 3,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 39,
    CategoryId = 3,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 40,
    CategoryId = 3,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 41,
    CategoryId = 3,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 42,
    CategoryId = 3,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 43,
    CategoryId = 3,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 44,
    CategoryId = 3,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 45,
    CategoryId = 4,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 46,
    CategoryId = 4,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 47,
    CategoryId = 4,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 48,
    CategoryId = 4,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 49,
    CategoryId = 4,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 50,
    CategoryId = 4,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 51,
    CategoryId = 4,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 52,
    CategoryId = 4,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 53,
    CategoryId = 4,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 54,
    CategoryId = 4,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 55,
    CategoryId = 4,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 56,
    CategoryId = 4,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 57,
    CategoryId = 4,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 58,
    CategoryId = 4,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 59,
    CategoryId = 4,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 60,
    CategoryId = 4,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 61,
    CategoryId = 4,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 62,
    CategoryId = 4,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 63,
    CategoryId = 4,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 64,
    CategoryId = 4,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 65,
    CategoryId = 2,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 66,
    CategoryId = 2,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 67,
    CategoryId = 2,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 68,
    CategoryId = 2,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 69,
    CategoryId = 2,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 70,
    CategoryId = 2,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 71,
    CategoryId = 2,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 72,
    CategoryId = 2,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 73,
    CategoryId = 2,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 74,
    CategoryId = 2,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 75,
    CategoryId = 2,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 76,
    CategoryId = 2,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 77,
    CategoryId = 2,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 78,
    CategoryId = 2,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 79,
    CategoryId = 2,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
new ProductCategory
{
    ProductId = 80,
    CategoryId = 2,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
},
            };

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
                var coupons = new[]
                {
                    new Coupon
                    {
                        Code = "JUNESALE20",
                        Description = "20% off all orders over $75",
                        Type = CouponType.Percentage,
                        Value = 10,
                        MinimumOrderAmount = 50,
                        IsActive = true,
                        IsForNewUsersOnly = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        DiscountAmount = null,
                    },
                    new Coupon
                    {
                        Code = "SAVE15OFF",
                        Description = "Get $15 off your next purchase",
                        Type = CouponType.FixedAmount,
                        Value = 20,
                        MinimumOrderAmount = 100,
                        IsActive = true,
                        IsForNewUsersOnly = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        DiscountAmount = 15.00m
                    },
                    new Coupon
                    {
                        Code = "WKNDFLASH",
                        Description = "10% off on electronics this weekend",
                        Type = CouponType.Percentage,
                        Value = 15,
                        MinimumOrderAmount = 75,
                        IsActive = true,
                        IsForNewUsersOnly = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        DiscountAmount = null
                    },
                    new Coupon
                    {
                        Code = "APRILDEAL",
                        Description = "Flat $50 off on orders over $250",
                        Type = CouponType.FixedAmount,
                        Value = 0,
                        MinimumOrderAmount = 50,
                        IsActive = true,
                        IsForNewUsersOnly = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        DiscountAmount = 50.00m
                    },
                    new Coupon
                    {
                        Code = "ECOFRIENDLY",
                        Description = "12% off sustainable products",
                        Type = CouponType.FixedAmount,
                        Value = 0,
                        MinimumOrderAmount = 50,
                        IsActive = true,
                        IsForNewUsersOnly = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        DiscountAmount = null
                    },
                    new Coupon
                    {
                        Code = "LOYALTYDISC",
                        Description = "Special 5% discount for returning customers",
                        Type = CouponType.FixedAmount,
                        Value = 0,
                        MinimumOrderAmount = 50,
                        IsActive = true,
                        IsForNewUsersOnly = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        DiscountAmount = null
                    },
                    new Coupon
                    {
                        Code = "HOLIDAY2025",
                        Description = "Flat $10 off any purchase",
                        Type = CouponType.FixedAmount,
                        Value = 0,
                        MinimumOrderAmount = 50,
                        IsActive = true,
                        IsForNewUsersOnly = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        DiscountAmount = 10.00m
                    },
                    new Coupon
                    {
                        Code = "NEWBIE25",
                        Description = "25% off your very first order",
                        Type = CouponType.FixedAmount,
                        Value = 0,
                        MinimumOrderAmount = 50,
                        IsActive = true,
                        IsForNewUsersOnly = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        DiscountAmount = null
                    },
                    new Coupon
                    {
                        Code = "WELCOMEBACK",
                        Description = "Get $20 off your initial order over $75",
                        Type = CouponType.FixedAmount,
                        Value = 0,
                        MinimumOrderAmount = 50,
                        IsActive = true,
                        IsForNewUsersOnly = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        DiscountAmount = 20.00m
                    },
                };

                _context.Coupons.AddRange(coupons);
                Console.WriteLine($"Created {coupons.Length} coupons");
            }
        }
    }
}