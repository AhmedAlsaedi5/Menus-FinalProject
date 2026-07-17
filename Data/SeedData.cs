using menus_project.Constants;
using menus_project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace menus_project.Data
{
    public static class SeedData
    {
        public static async Task SeedRolesAndAdmin(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { AppRoles.Admin, AppRoles.RestaurantOwner, AppRoles.User };

            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));

            }


            string adminEmail = "admin@menus.com";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                ApplicationUser admin = new ApplicationUser
                {
                    FirstName = "admin",
                    LastName = "menus",
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "Admin123");

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, AppRoles.Admin);
            }
        }

        public static void SeedCategories(AppDbContext context)
        {
            if(!context.RestaurantCategories.Any())
            {
                List<RestaurantCategory> restaurantCategories = new List<RestaurantCategory>
                {
                     new RestaurantCategory { Name = "Burgers",      NameInArabic = "برجر" },
                     new RestaurantCategory { Name = "Pizza",        NameInArabic = "بيتزا" },
                     new RestaurantCategory { Name = "Grills",       NameInArabic = "مشويات" },
                     new RestaurantCategory { Name = "Seafood",      NameInArabic = "مأكولات بحرية" },
                     new RestaurantCategory { Name = "Shawarma",     NameInArabic = "شاورما" },
                     new RestaurantCategory { Name = "Sushi",        NameInArabic = "سوشي" },
                     new RestaurantCategory { Name = "Pasta",        NameInArabic = "باستا" },
                     new RestaurantCategory { Name = "Sandwiches",   NameInArabic = "سندويشات" },
                     new RestaurantCategory { Name = "Healthy",      NameInArabic = "صحي" },
                     new RestaurantCategory { Name = "Desserts",     NameInArabic = "حلويات" },
                     new RestaurantCategory { Name = "Breakfast",    NameInArabic = "فطور" },
                     new RestaurantCategory { Name = "Beverages",    NameInArabic = "مشروبات" }
                };

                context.RestaurantCategories.AddRange(restaurantCategories);
                context.SaveChanges();
            }

            if(!context.MenuItemCategories.Any())
            {
                List<MenuItemCategory> menuItemCategories = new List<MenuItemCategory>
                {
                                new MenuItemCategory { Name = "Main Course", NameInArabic = "طبق رئيسي" },
                                  new MenuItemCategory { Name = "Side Dish",   NameInArabic = "طبق جانبي"},
                                  new MenuItemCategory { Name = "Beverages",   NameInArabic = "مشروبات"},
                                  new MenuItemCategory { Name = "Desserts",    NameInArabic = "حلويات" }
                };

                context.MenuItemCategories.AddRange(menuItemCategories);
                context.SaveChanges();
            }

        }

        public static async Task SeedSampleDataAsync(
        AppDbContext context,
        UserManager<ApplicationUser> userManager,
        IWebHostEnvironment environment)
        {
            if (context.Restaurants.Any()) return; // تم البذر مسبقاً

            // ── 1. جلب التصنيفات الموجودة ──────────────────────
            var catBurger = context.RestaurantCategories.FirstOrDefault(c => c.Name == "Burgers");
            var catPizza = context.RestaurantCategories.FirstOrDefault(c => c.Name == "Pizza");
            var catGrills = context.RestaurantCategories.FirstOrDefault(c => c.Name == "Grills");
            var catShawarma = context.RestaurantCategories.FirstOrDefault(c => c.Name == "Shawarma");
            var catSushi = context.RestaurantCategories.FirstOrDefault(c => c.Name == "Sushi");
            var catFast = context.RestaurantCategories.FirstOrDefault(c => c.Name == "Fast Food");

            var mainCourse = context.MenuItemCategories.FirstOrDefault(c => c.Name == "Main Course");
            var sideDish = context.MenuItemCategories.FirstOrDefault(c => c.Name == "Side Dish");
            var beverages = context.MenuItemCategories.FirstOrDefault(c => c.Name == "Beverages");
            var desserts = context.MenuItemCategories.FirstOrDefault(c => c.Name == "Desserts");

            // ── 2. تحميل الصور ─────────────────────────────────
            Console.WriteLine("⬇️  جاري تحميل الصور...");

            var images = new Dictionary<string, string>
            {
                // صور المطاعم
                ["r_burger"] = await DownloadImageAsync("https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=600&q=80", "r_burger_house.jpg", environment),
                ["r_pizza"] = await DownloadImageAsync("https://images.unsplash.com/photo-1574071318508-1cdbab80d002?w=600&q=80", "r_pizza_corner.jpg", environment),
                ["r_grills"] = await DownloadImageAsync("https://images.unsplash.com/photo-1555939594-58d7cb561ad1?w=600&q=80","r_samarqand.jpg", environment),
                ["r_shawarma"] = await DownloadImageAsync("https://images.unsplash.com/photo-1599487488170-d11ec9c172f0?w=600&q=80", "r_shawarma_plus.jpg", environment),
                ["r_sushi"] = await DownloadImageAsync("https://images.unsplash.com/photo-1579584425555-c3ce17fd4351?w=600&q=80", "r_sushi_time.jpg", environment),

                // صور أطباق البرجر
                ["m_classic"] = await DownloadImageAsync("https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=400&q=80", "m_classic_burger.jpg", environment),
                ["m_cheese"] = await DownloadImageAsync("https://images.unsplash.com/photo-1553979459-d2229ba7433b?w=400&q=80", "m_cheese_burger.jpg", environment),
                ["m_chicken"] = await DownloadImageAsync("https://images.unsplash.com/photo-1562967914-608f82629710?w=400&q=80", "m_crispy_chicken.jpg", environment),
                ["m_fries"] = await DownloadImageAsync("https://images.unsplash.com/photo-1576107232684-1279f390859f?w=400&q=80","m_fries.jpg", environment),
                ["m_cola"] = await DownloadImageAsync("https://images.unsplash.com/photo-1554866585-cd94860890b7?w=400&q=80", "m_cola.jpg", environment),
                ["m_shake"] = await DownloadImageAsync("https://images.unsplash.com/photo-1572490122747-3968b75cc699?w=400&q=80", "m_milkshake.jpg", environment),

                // صور أطباق البيتزا
                ["m_marg"] = await DownloadImageAsync("https://images.unsplash.com/photo-1574071318508-1cdbab80d002?w=400&q=80", "m_margherita.jpg", environment),
                ["m_pepperoni"] = await DownloadImageAsync("https://images.unsplash.com/photo-1565299624946-b28f40a0ae38?w=400&q=80", "m_pepperoni.jpg", environment),
                ["m_bbq"] = await DownloadImageAsync("https://images.unsplash.com/photo-1513104890138-7c749659a591?w=400&q=80", "m_bbq_pizza.jpg", environment),
                ["m_garlic"] = await DownloadImageAsync("https://images.unsplash.com/photo-1619870282569-51c55c0718b1?w=400&q=80", "m_garlic_bread.jpg", environment),

                // صور أطباق المشاوي
                ["m_mixed"] = await DownloadImageAsync("https://images.unsplash.com/photo-1529193591184-b1d58069ecdd?w=400&q=80", "m_mixed_grill.jpg", environment),
                ["m_kebab"] = await DownloadImageAsync("https://images.unsplash.com/photo-1544025162-d76538b2a681?w=400&q=80", "m_kebab.jpg", environment),
                ["m_gchicken"] = await DownloadImageAsync("https://images.unsplash.com/photo-1598515214211-89d3c73ae83b?w=400&q=80", "m_grilled_chicken.jpg", environment),
                ["m_hummus"] = await DownloadImageAsync("https://images.unsplash.com/photo-1547592180-85f173990554?w=400&q=80", "m_hummus.jpg", environment),
                ["m_ayran"] = await DownloadImageAsync("https://images.unsplash.com/photo-1550583724-b2692b85b150?w=400&q=80", "m_ayran.jpg", environment),

                // صور أطباق الشاورما
                ["m_cshaw"] = await DownloadImageAsync("https://images.unsplash.com/photo-1599487488170-d11ec9c172f0?w=400&q=80", "m_chicken_shawarma.jpg", environment),
                ["m_mshaw"] = await DownloadImageAsync("https://images.unsplash.com/photo-1633321702518-7feccafb94d5?w=400&q=80", "m_meat_shawarma.jpg", environment),
                ["m_falafel"] = await DownloadImageAsync("https://images.unsplash.com/photo-1593001874117-c99c800e3eb7?w=400&q=80", "m_falafel.jpg", environment),

                // صور أطباق السوشي
                ["m_salmon"] = await DownloadImageAsync("https://images.unsplash.com/photo-1579584425555-c3ce17fd4351?w=400&q=80", "m_salmon_roll.jpg", environment),
                ["m_cali"] = await DownloadImageAsync("https://images.unsplash.com/photo-1617196034183-421b4040ed20?w=400&q=80", "m_california_roll.jpg", environment),
                ["m_miso"] = await DownloadImageAsync("https://images.unsplash.com/photo-1547592180-85f173990554?w=400&q=80", "m_miso_soup.jpg", environment),
                ["m_edamame"] = await DownloadImageAsync("https://images.unsplash.com/photo-1564671546655-9b1eee6e3f8e?w=400&q=80", "m_edamame.jpg", environment),
            };

            Console.WriteLine("✅ تم تحميل الصور");

            // ── 3. إنشاء أصحاب المطاعم ────────────────────────
            var ownerData = new[]
            {
                ("owner1@menus.com", "أحمد",    "الرشيدي",  "0501234561"),
                ("owner2@menus.com", "محمد",    "الغامدي",  "0501234562"),
                ("owner3@menus.com", "خالد",    "العتيبي",  "0501234563"),
                ("owner4@menus.com", "عبدالله", "الزهراني", "0501234564"),
                ("owner5@menus.com", "عمر",     "الشهري",   "0501234565"),
            };

            var ownerUsers = new List<ApplicationUser>();
            foreach (var (email, first, last, phone) in ownerData)
            {
                var existing = await userManager.FindByEmailAsync(email);
                if (existing != null)
                {
                    ownerUsers.Add(existing);
                    continue;
                }
                var owner = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = first,
                    LastName = last,
                    PhoneNumber = phone,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(owner, "Owner@12345");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(owner, AppRoles.RestaurantOwner);
                ownerUsers.Add(owner);
            }

            // ── 4. إنشاء المطاعم مع الأطباق والمواقع ──────────
            var restaurants = new List<Restaurant>
            {
                // ─── 1. برجر هاوس ───────────────────────────────
                new Restaurant
                {
                    Name          = "Burger House",
                    NameInArabic  = "برجر هاوس",
                    Image_url     = images["r_burger"],
                    AverageRating = 4.5m,
                    ReviewCount   = 128,
                    IsActive      = true,
                    UserId        = ownerUsers[0].Id,
                    RestaurantCategories = new List<RestaurantCategory>
                        { catBurger }.Where(c => c != null).ToList(),
                    Locations = new List<Location>
                    {
                        new Location
                        {
                            Address   = "طريق الملك فهد، حي العليا، الرياض",
                            Latitude  = 24.6877m,
                            Longitude = 46.7219m
                        }
                    },
                    MenuItems = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Name                = "Classic Burger",
                            NameInArabic        = "برجر كلاسيك",
                            Description         = "Classic beef burger with fresh lettuce, tomato and pickles",
                            DescriptionInArabic = "برجر لحم بقري كلاسيكي مع خس طازج وطماطم وخيار مخلل",
                            Price               = 28.00m,
                            Image_url           = images["m_classic"],
                            MenuItemCategoryId  = mainCourse?.Id ?? 1,
                            AverageRating       = 4.5m,
                            ReviewCount         = 45,
                            IsAvailable         = true
                        },
                        new MenuItem
                        {
                            Name                = "Double Cheese Burger",
                            NameInArabic        = "دبل تشيز برجر",
                            Description         = "Double patty with cheddar and special house sauce",
                            DescriptionInArabic = "باتي مضاعف مع جبن شيدر وصلصة منزلية خاصة",
                            Price               = 38.00m,
                            Image_url           = images["m_cheese"],
                            MenuItemCategoryId  = mainCourse?.Id ?? 1,
                            AverageRating       = 4.7m,
                            ReviewCount         = 62,
                            IsAvailable         = true
                        },
                        new MenuItem
                        {
                            Name                = "Crispy Chicken Sandwich",
                            NameInArabic        = "ساندويش كريسبي تشيكن",
                            Description         = "Crispy fried chicken breast with coleslaw",
                            DescriptionInArabic = "صدر دجاج مقلي مقرمش مع كول سلو",
                            Price               = 30.00m,
                            Image_url           = images["m_chicken"],
                            MenuItemCategoryId  = mainCourse?.Id ?? 1,
                            AverageRating       = 4.3m,
                            ReviewCount         = 38,
                            IsAvailable         = true
                        },
                        new MenuItem
                        {
                            Name                = "French Fries",
                            NameInArabic        = "بطاطس مقلية",
                            Description         = "Golden crispy french fries with ketchup",
                            DescriptionInArabic = "بطاطس مقلية ذهبية مقرمشة مع كاتشب",
                            Price               = 12.00m,
                            Image_url           = images["m_fries"],
                            MenuItemCategoryId  = sideDish?.Id ?? 2,
                            AverageRating       = 4.2m,
                            ReviewCount         = 29,
                            IsAvailable         = true
                        },
                        new MenuItem
                        {
                            Name                = "Cola",
                            NameInArabic        = "كولا",
                            Description         = "Ice cold cola soft drink",
                            DescriptionInArabic = "مشروب كولا غازي بارد مع ثلج",
                            Price               = 7.00m,
                            Image_url           = images["m_cola"],
                            MenuItemCategoryId  = beverages?.Id ?? 3,
                            AverageRating       = 4.0m,
                            ReviewCount         = 15,
                            IsAvailable         = true
                        },
                        new MenuItem
                        {
                            Name                = "Chocolate Milkshake",
                            NameInArabic        = "ميلك شيك شوكولاتة",
                            Description         = "Creamy rich chocolate milkshake",
                            DescriptionInArabic = "ميلك شيك شوكولاتة كريمي غني",
                            Price               = 18.00m,
                            Image_url           = images["m_shake"],
                            MenuItemCategoryId  = beverages?.Id ?? 3,
                            AverageRating       = 4.6m,
                            ReviewCount         = 33,
                            IsAvailable         = true
                        },
                    }
                },

                // ─── 2. بيتزا كورنر ─────────────────────────────
                new Restaurant
                {
                    Name          = "Pizza Corner",
                    NameInArabic  = "بيتزا كورنر",
                    Image_url     = images["r_pizza"],
                    AverageRating = 4.3m,
                    ReviewCount   = 95,
                    IsActive      = true,
                    UserId        = ownerUsers[1].Id,
                    RestaurantCategories = new List<RestaurantCategory>
                        { catPizza }.Where(c => c != null).ToList(),
                    Locations = new List<Location>
                    {
                        new Location
                        {
                            Address   = "شارع العليا، حي السليمانية، الرياض",
                            Latitude  = 24.7136m,
                            Longitude = 46.6753m
                        }
                    },
                    MenuItems = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Name                = "Margherita Pizza",
                            NameInArabic        = "بيتزا مارغريتا",
                            Description         = "Classic margherita with San Marzano tomato sauce and mozzarella",
                            DescriptionInArabic = "مارغريتا كلاسيكية بصلصة طماطم سان مارزانو وموتزاريلا",
                            Price               = 32.00m,
                            Image_url           = images["m_marg"],
                            MenuItemCategoryId  = mainCourse?.Id ?? 1,
                            AverageRating       = 4.4m,
                            ReviewCount         = 52,
                            IsAvailable         = true
                        },
                        new MenuItem
                        {
                            Name                = "Pepperoni Pizza",
                            NameInArabic        = "بيتزا بيبروني",
                            Description         = "Loaded with premium pepperoni and extra mozzarella",
                            DescriptionInArabic = "محملة ببيبروني فاخر وموتزاريلا إضافية",
                            Price               = 42.00m,
                            Image_url           = images["m_pepperoni"],
                            MenuItemCategoryId  = mainCourse?.Id ?? 1,
                            AverageRating       = 4.6m,
                            ReviewCount         = 71,
                            IsAvailable         = true
                        },
                        new MenuItem
                        {
                            Name                = "BBQ Chicken Pizza",
                            NameInArabic        = "بيتزا دجاج باربيكيو",
                            Description         = "Grilled chicken strips with smoky BBQ sauce",
                            DescriptionInArabic = "شرائح دجاج مشوي مع صلصة باربيكيو مدخنة",
                            Price               = 45.00m,
                            Image_url           = images["m_bbq"],
                            MenuItemCategoryId  = mainCourse?.Id ?? 1,
                            AverageRating       = 4.5m,
                            ReviewCount         = 48,
                            IsAvailable         = true
                        },
                        new MenuItem
                        {
                            Name                = "Garlic Bread",
                            NameInArabic        = "خبز الثوم",
                            Description         = "Toasted artisan bread with garlic herb butter",
                            DescriptionInArabic = "خبز أرتيزان محمص بزبدة الثوم والأعشاب",
                            Price               = 15.00m,
                            Image_url           = images["m_garlic"],
                            MenuItemCategoryId  = sideDish?.Id ?? 2,
                            AverageRating       = 4.1m,
                            ReviewCount         = 27,
                            IsAvailable         = true
                        },
                        new MenuItem
                        {
                            Name                = "Cola",
                            NameInArabic        = "كولا",
                            Description         = "Ice cold cola soft drink",
                            DescriptionInArabic = "مشروب كولا غازي بارد",
                            Price               = 7.00m,
                            Image_url           = images["m_cola"],
                            MenuItemCategoryId  = beverages?.Id ?? 3,
                            AverageRating       = 4.0m,
                            ReviewCount         = 18,
                            IsAvailable         = true
                        },
                    }
                },

                // ─── 3. مطعم السمرقند ────────────────────────────
                new Restaurant
                {
                    Name          = "Al Samarqand",
                    NameInArabic  = "مطعم السمرقند",
                    Image_url     = images["r_grills"],
                    AverageRating = 4.8m,
                    ReviewCount   = 203,
                    IsActive      = true,
                    UserId        = ownerUsers[2].Id,
                    RestaurantCategories = new List<RestaurantCategory>
                        { catGrills }.Where(c => c != null).ToList(),
                    Locations = new List<Location>
                    {
                        new Location
                        {
                            Address   = "طريق الأمير محمد بن عبدالعزيز، حي الورود، الرياض",
                            Latitude  = 24.6508m,
                            Longitude = 46.7102m
                        }
                    },
                    MenuItems = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Name                = "Mixed Grill Platter",
                            NameInArabic        = "طبق مشاوي مشكلة",
                            Description         = "Assorted premium grilled meats - lamb, chicken and kofta",
                            DescriptionInArabic = "تشكيلة من أفخر المشاوي - ضأن ودجاج وكفتة",
                            Price               = 89.00m,
                            Image_url           = images["m_mixed"],
                            MenuItemCategoryId  = mainCourse?.Id ?? 1,
                            AverageRating       = 4.8m,
                            ReviewCount         = 87,
                            IsAvailable         = true
                        },
                        new MenuItem
                        {
                            Name                = "Lamb Kebab",
                            NameInArabic        = "كباب لحم ضأن",
                            Description         = "Tender minced lamb kebab on charcoal grill",
                            DescriptionInArabic = "كباب لحم ضأن مفروم طري على الفحم",
                            Price               = 65.00m,
                            Image_url           = images["m_kebab"],
                            MenuItemCategoryId  = mainCourse?.Id ?? 1,
                            AverageRating       = 4.9m,
                            ReviewCount         = 94,
                            IsAvailable         = true
                        },
                        new MenuItem
                        {
                            Name                = "Grilled Whole Chicken",
                            NameInArabic        = "دجاجة مشوية كاملة",
                            Description         = "Marinated whole chicken grilled over charcoal",
                            DescriptionInArabic = "دجاجة كاملة متبلة مشوية على الفحم",
                            Price               = 55.00m,
                            Image_url           = images["m_gchicken"],
                            MenuItemCategoryId  = mainCourse?.Id ?? 1,
                            AverageRating       = 4.7m,
                            ReviewCount         = 76,
                            IsAvailable         = true
                        },
                        new MenuItem
                        {
                            Name                = "Hummus",
                            NameInArabic        = "حمص",
                            Description         = "Creamy homemade hummus with extra virgin olive oil",
                            DescriptionInArabic = "حمص منزلي كريمي مع زيت زيتون بكر ممتاز",
                            Price               = 18.00m,
                            Image_url           = images["m_hummus"],
                            MenuItemCategoryId  = sideDish?.Id ?? 2,
                            AverageRating       = 4.6m,
                            ReviewCount         = 52,
                            IsAvailable         = true
                        },
                        new MenuItem
                        {
                            Name                = "Ayran",
                            NameInArabic        = "عيران",
                            Description         = "Traditional cold yogurt drink",
                            DescriptionInArabic = "مشروب اللبن التقليدي البارد",
                            Price               = 8.00m,
                            Image_url           = images["m_ayran"],
                            MenuItemCategoryId  = beverages?.Id ?? 3,
                            AverageRating       = 4.3m,
                            ReviewCount         = 31,
                            IsAvailable         = true
                        },
                    }
                },

                // ─── 4. شاورما بلس ──────────────────────────────
                new Restaurant
                {
                    Name          = "Shawarma Plus",
                    NameInArabic  = "شاورما بلس",
                    Image_url     = images["r_shawarma"],
                    AverageRating = 4.2m,
                    ReviewCount   = 156,
                    IsActive      = true,
                    UserId        = ownerUsers[3].Id,
                    RestaurantCategories = new List<RestaurantCategory>
                        { catShawarma }.Where(c => c != null).ToList(),
                    Locations = new List<Location>
                    {
                        new Location
                        {
                            Address   = "طريق الدائري الشمالي، حي النزهة، الرياض",
                            Latitude  = 24.7741m,
                            Longitude = 46.7385m
                        }
                    },
                    MenuItems = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Name                = "Chicken Shawarma",
                            NameInArabic        = "شاورما دجاج",
                            Description         = "Juicy marinated chicken shawarma with garlic sauce and pickles",
                            DescriptionInArabic = "شاورما دجاج متبل عصيري مع صلصة ثوم ومخللات",
                            Price               = 18.00m,
                            Image_url           = images["m_cshaw"],
                            MenuItemCategoryId  = mainCourse?.Id ?? 1,
                            AverageRating       = 4.3m,
                            ReviewCount         = 89,
                            IsAvailable         = true
                        },
                        new MenuItem
                        {
                            Name                = "Meat Shawarma",
                            NameInArabic        = "شاورما لحم",
                            Description         = "Tender seasoned beef shawarma with tahini sauce",
                            DescriptionInArabic = "شاورما لحم بقري متبل طري مع صلصة طحينة",
                            Price               = 22.00m,
                            Image_url           = images["m_mshaw"],
                            MenuItemCategoryId  = mainCourse?.Id ?? 1,
                            AverageRating       = 4.4m,
                            ReviewCount         = 73,
                            IsAvailable         = true
                        },
                        new MenuItem
                        {
                            Name                = "Falafel Sandwich",
                            NameInArabic        = "ساندويش فلافل",
                            Description         = "Crispy falafel with fresh vegetables and tahini",
                            DescriptionInArabic = "فلافل مقرمشة مع خضروات طازجة وطحينة",
                            Price               = 14.00m,
                            Image_url           = images["m_falafel"],
                            MenuItemCategoryId  = mainCourse?.Id ?? 1,
                            AverageRating       = 4.1m,
                            ReviewCount         = 45,
                            IsAvailable         = true
                        },
                        new MenuItem
                        {
                            Name                = "Ayran",
                            NameInArabic        = "عيران",
                            Description         = "Traditional cold yogurt drink",
                            DescriptionInArabic = "مشروب اللبن التقليدي البارد",
                            Price               = 8.00m,
                            Image_url           = images["m_ayran"],
                            MenuItemCategoryId  = beverages?.Id ?? 3,
                            AverageRating       = 4.0m,
                            ReviewCount         = 28,
                            IsAvailable         = true
                        },
                    }
                },

                // ─── 5. سوشي تايم ───────────────────────────────
                new Restaurant
                {
                    Name          = "Sushi Time",
                    NameInArabic  = "سوشي تايم",
                    Image_url     = images["r_sushi"],
                    AverageRating = 4.6m,
                    ReviewCount   = 87,
                    IsActive      = true,
                    UserId        = ownerUsers[4].Id,
                    RestaurantCategories = new List<RestaurantCategory>
                        { catSushi }.Where(c => c != null).ToList(),
                    Locations = new List<Location>
                    {
                        new Location
                        {
                            Address   = "شارع التحلية، حي النخيل، الرياض",
                            Latitude  = 24.6234m,
                            Longitude = 46.8012m
                        }
                    },
                    MenuItems = new List<MenuItem>
                    {
                        new MenuItem
                        {
                            Name                = "Salmon Roll",
                            NameInArabic        = "رول سلمون",
                            Description         = "Fresh Norwegian salmon roll with creamy avocado",
                            DescriptionInArabic = "رول سلمون نرويجي طازج مع أفوكادو كريمي",
                            Price               = 48.00m,
                            Image_url           = images["m_salmon"],
                            MenuItemCategoryId  = mainCourse?.Id ?? 1,
                            AverageRating       = 4.7m,
                            ReviewCount         = 52,
                            IsAvailable         = true
                        },
                        new MenuItem
                        {
                            Name                = "California Roll",
                            NameInArabic        = "رول كاليفورنيا",
                            Description         = "Classic California roll with imitation crab and avocado",
                            DescriptionInArabic = "رول كاليفورنيا الكلاسيكي مع كراب وأفوكادو",
                            Price               = 38.00m,
                            Image_url           = images["m_cali"],
                            MenuItemCategoryId  = mainCourse?.Id ?? 1,
                            AverageRating       = 4.5m,
                            ReviewCount         = 41,
                            IsAvailable         = true
                        },
                        new MenuItem
                        {
                            Name                = "Miso Soup",
                            NameInArabic        = "شوربة ميسو",
                            Description         = "Traditional Japanese miso soup with tofu and seaweed",
                            DescriptionInArabic = "شوربة ميسو يابانية تقليدية مع توفو وأعشاب بحرية",
                            Price               = 15.00m,
                            Image_url           = images["m_miso"],
                            MenuItemCategoryId  = sideDish?.Id ?? 2,
                            AverageRating       = 4.3m,
                            ReviewCount         = 29,
                            IsAvailable         = true
                        },
                        new MenuItem
                        {
                            Name                = "Edamame",
                            NameInArabic        = "إيدامامي",
                            Description         = "Steamed young soybeans with sea salt",
                            DescriptionInArabic = "فول الصويا الطازج المطبوخ بالبخار مع ملح البحر",
                            Price               = 20.00m,
                            Image_url           = images["m_edamame"],
                            MenuItemCategoryId  = sideDish?.Id ?? 2,
                            AverageRating       = 4.4m,
                            ReviewCount         = 35,
                            IsAvailable         = true
                        },
                        new MenuItem
                        {
                            Name                = "Green Tea",
                            NameInArabic        = "شاي أخضر",
                            Description         = "Traditional hot Japanese green tea",
                            DescriptionInArabic = "شاي أخضر ياباني تقليدي ساخن",
                            Price               = 12.00m,
                            Image_url           = images["m_cola"],
                            MenuItemCategoryId  = beverages?.Id ?? 3,
                            AverageRating       = 4.2m,
                            ReviewCount         = 22,
                            IsAvailable         = true
                        },
                    }
                },
            };

            context.Restaurants.AddRange(restaurants);
            await context.SaveChangesAsync();
            Console.WriteLine("✅ تم حفظ المطاعم والأطباق والمواقع");

            // ── 5. إنشاء المستخدمين العاديين ───────────────────
            var userData = new[]
            {
                ("user1@menus.com", "عمر",    "الحربي",   "0521234561"),
                ("user2@menus.com", "فاطمة",   "القحطاني", "0521234562"),
                ("user3@menus.com", "خالد",    "الدوسري",  "0521234563"),
                ("user4@menus.com", "لينا",    "المطيري",  "0521234564"),
                ("user5@menus.com", "هنا",     "الشمري",   "0521234565"),
            };

            var regularUsers = new List<ApplicationUser>();
            foreach (var (email, first, last, phone) in userData)
            {
                var existing = await userManager.FindByEmailAsync(email);
                if (existing != null)
                {
                    regularUsers.Add(existing);
                    continue;
                }
                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = first,
                    LastName = last,
                    PhoneNumber = phone,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(user, "User@12345");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, AppRoles.User);
                regularUsers.Add(user);
            }

            // ── 6. تقييمات المطاعم ──────────────────────────────
            var restaurantReviews = new List<RestaurantReview>
            {
                // Burger House
                new RestaurantReview { RestaurantId = restaurants[0].Id, UserId = regularUsers[0].Id, Rating = 5, Comment = "برجر رائع ولذيذ جداً، سأعود مرة أخرى بالتأكيد!", CreatedAt = DateTime.UtcNow.AddDays(-10) },
                new RestaurantReview { RestaurantId = restaurants[0].Id, UserId = regularUsers[1].Id, Rating = 4, Comment = "جودة ممتازة والخدمة سريعة، أنصح بتجربته", CreatedAt = DateTime.UtcNow.AddDays(-7) },
                new RestaurantReview { RestaurantId = restaurants[0].Id, UserId = regularUsers[2].Id, Rating = 5, Comment = "أفضل برجر في الرياض بدون منازع، العجينة طازجة", CreatedAt = DateTime.UtcNow.AddDays(-3) },

                // Pizza Corner
                new RestaurantReview { RestaurantId = restaurants[1].Id, UserId = regularUsers[0].Id, Rating = 4, Comment = "بيتزا شهية والعجينة رقيقة وطازجة", CreatedAt = DateTime.UtcNow.AddDays(-15) },
                new RestaurantReview { RestaurantId = restaurants[1].Id, UserId = regularUsers[3].Id, Rating = 4, Comment = "تجربة ممتعة والأسعار معقولة جداً", CreatedAt = DateTime.UtcNow.AddDays(-5) },
                new RestaurantReview { RestaurantId = restaurants[1].Id, UserId = regularUsers[4].Id, Rating = 3, Comment = "البيتزا جيدة لكن التوصيل يستغرق وقتاً", CreatedAt = DateTime.UtcNow.AddDays(-2) },

                // Al Samarqand
                new RestaurantReview { RestaurantId = restaurants[2].Id, UserId = regularUsers[1].Id, Rating = 5, Comment = "أفضل مشاوي تذوقتها في حياتي! اللحم طري والنكهة رائعة", CreatedAt = DateTime.UtcNow.AddDays(-20) },
                new RestaurantReview { RestaurantId = restaurants[2].Id, UserId = regularUsers[2].Id, Rating = 5, Comment = "الكباب طري ولذيذ والخدمة راقية، مطعم ممتاز", CreatedAt = DateTime.UtcNow.AddDays(-12) },
                new RestaurantReview { RestaurantId = restaurants[2].Id, UserId = regularUsers[4].Id, Rating = 5, Comment = "مطعم راقٍ بامتياز، يستحق كل ريال", CreatedAt = DateTime.UtcNow.AddDays(-2) },

                // Shawarma Plus
                new RestaurantReview { RestaurantId = restaurants[3].Id, UserId = regularUsers[3].Id, Rating = 4, Comment = "شاورما لذيذة وسريعة التحضير، صلصة الثوم رائعة", CreatedAt = DateTime.UtcNow.AddDays(-8) },
                new RestaurantReview { RestaurantId = restaurants[3].Id, UserId = regularUsers[4].Id, Rating = 4, Comment = "أسعار مناسبة وطعم جيد، مناسب للعائلة", CreatedAt = DateTime.UtcNow.AddDays(-4) },

                // Sushi Time
                new RestaurantReview { RestaurantId = restaurants[4].Id, UserId = regularUsers[0].Id, Rating = 5, Comment = "سوشي طازج ورائع! أفضل سوشي جربته في السعودية", CreatedAt = DateTime.UtcNow.AddDays(-6) },
                new RestaurantReview { RestaurantId = restaurants[4].Id, UserId = regularUsers[2].Id, Rating = 4, Comment = "تجربة سوشي مميزة في الرياض، السلمون طازج جداً", CreatedAt = DateTime.UtcNow.AddDays(-1) },
            };

            context.RestaurantReviews.AddRange(restaurantReviews);
            await context.SaveChangesAsync();
            Console.WriteLine("✅ تم حفظ تقييمات المطاعم");

            // ── 7. تقييمات الأطباق ──────────────────────────────
            var rng = new Random(42);
            var allItems = restaurants.SelectMany(r => r.MenuItems).ToList();
            var menuItemReviews = new List<MenuItemReview>();

            foreach (var item in allItems)
            {
                var reviewers = regularUsers.Take(rng.Next(2, 5)).ToList();
                foreach (var user in reviewers)
                {
                    menuItemReviews.Add(new MenuItemReview
                    {
                        MenuItemId = item.Id,
                        UserId = user.Id,
                        Rating = (byte)rng.Next(3, 6),
                        CreatedAt = DateTime.UtcNow.AddDays(-rng.Next(1, 30))
                    });
                }
            }

            context.MenuItemReviews.AddRange(menuItemReviews);
            await context.SaveChangesAsync();
            Console.WriteLine("✅ تم حفظ تقييمات الأطباق");

            // ── 8. المطاعم المفضلة ──────────────────────────────
            var usersWithFavorites = await context.Users
                .Include(u => u.FavoriteRestaurants)
                .Where(u => regularUsers.Select(r => r.Id).Contains(u.Id))
                .ToListAsync();

            var favorites = new Dictionary<int, int[]>
            {
                [0] = new[] { 0, 2 },       // سارة    → برجر هاوس + السمرقند
                [1] = new[] { 1, 4 },       // فاطمة   → بيتزا + سوشي
                [2] = new[] { 2 },          // نورا    → السمرقند
                [3] = new[] { 0, 3 },       // لينا    → برجر + شاورما
                [4] = new[] { 4, 2 },       // هنا     → سوشي + السمرقند
            };

            for (int i = 0; i < usersWithFavorites.Count; i++)
            {
                if (!favorites.ContainsKey(i)) continue;
                foreach (var idx in favorites[i])
                    usersWithFavorites[i].FavoriteRestaurants.Add(restaurants[idx]);
            }

            await context.SaveChangesAsync();
            Console.WriteLine("✅ تم حفظ المفضلة");
            Console.WriteLine("🎉 اكتمل بذر البيانات بنجاح!");
        }

        // ════════════════════════════════════════════════════════
        //  تحميل الصور من الإنترنت
        // ════════════════════════════════════════════════════════
        private static async Task<string> DownloadImageAsync(
            string imageUrl,
            string fileName,
            IWebHostEnvironment environment)
        {
            try
            {
                var folder = Path.Combine(environment.WebRootPath, "Images");
                Directory.CreateDirectory(folder);

                var filePath = Path.Combine(folder, fileName);
                if (File.Exists(filePath))
                    return "" + fileName;

                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(30);
                client.DefaultRequestHeaders.Add("User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");

                var bytes = await client.GetByteArrayAsync(imageUrl);
                await File.WriteAllBytesAsync(filePath, bytes);

                Console.WriteLine($"  ⬇️  {fileName}");
                return "" + fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠️  فشل تحميل {fileName}: {ex.Message}");
                return "placeholder.jpg";
            }
        }
    
}
}
