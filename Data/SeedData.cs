using Microsoft.EntityFrameworkCore;
using DropThisSite.Models;

namespace DropThisSite.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
            // 1. StatusOrders
            if (!context.StatusOrders.Any())
            {
                context.StatusOrders.AddRange(
                    new StatusOrder { NameStatusOrder = "Новый" },
                    new StatusOrder { NameStatusOrder = "В процессе" },
                    new StatusOrder { NameStatusOrder = "Выполнен" },
                    new StatusOrder { NameStatusOrder = "Отменён" }
                );
                context.SaveChanges();
            }

            // 2. Roles
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role { NameRole = "Админ" },
                    new Role { NameRole = "Пользователь" }
                );
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { Login = "admin", Password = "admin1", Email = "admin@mail.ru", Phone = "+79315355564", IdRole = 1 }
                );
                context.SaveChanges();
            }

            // 3. JewelryTips
            if (!context.JewelryTips.Any())
            {
                context.JewelryTips.AddRange(
                    new JewelryTip { NameJewelryTip = "Кольца" },
                    new JewelryTip { NameJewelryTip = "Серьги" },
                    new JewelryTip { NameJewelryTip = "Ожерелья" },
                    new JewelryTip { NameJewelryTip = "Браслеты" }
                );
                context.SaveChanges();
            }

            // 4. Suppliers
            if (!context.Suppliers.Any())
            {
                context.Suppliers.AddRange(
                    new Supplier { NameSupplier = "ЮвелирСнаб", PhoneSupplier = "+78422540514", EmailSupplier = "uvelirsnab@gmail.com" },
                    new Supplier { NameSupplier = "ЗолотоВоДворе", PhoneSupplier = "+74718523154", EmailSupplier = "zolotishko@gmail.com" },
                    new Supplier { NameSupplier = "SEREBRO", PhoneSupplier = "+79991234588", EmailSupplier = "serebroo@mail.ru" }
                );
                context.SaveChanges();
            }

            // 5. Materials
            if (!context.Materials.Any())
            {
                context.Materials.AddRange(
                    new Material { NameMaterial = "Золото", Proba = 585 },
                    new Material { NameMaterial = "Серебро", Proba = 925 },
                    new Material { NameMaterial = "Белое золото", Proba = 585 }
                );
                context.SaveChanges();
            }

            // 6. Stones
            if (!context.Stones.Any())
            {
                context.Stones.AddRange(
                    new Stone { NameStone = "Бриллиант", ColorStone = "Прозрачный", WeightStone = 0.3F },
                    new Stone { NameStone = "Рубин", ColorStone = "Красный", WeightStone = 0.5F },
                    new Stone { NameStone = "Сапфир", ColorStone = "Синий", WeightStone = 0.45F },
                    new Stone { NameStone = "Без камня", ColorStone = "", WeightStone = 0F }
                );
                context.SaveChanges();
            }

            // 7. Jewelries
            if (!context.Jewelries.Any())
            {
                try
                {
                    context.Jewelries.AddRange(
                        new Jewelry { NameJewelry = "Кольцо из золота c бриллиантом", IdJewelryTip = 1, PriceJewelry = 45000, IdMaterial = 1, IdStone = 1, IdSupplier = 1 },
                        new Jewelry { NameJewelry = "Кольцо из серебра классика", IdJewelryTip = 1, PriceJewelry = 10000, IdMaterial = 2, IdStone = 3, IdSupplier = 1 },
                        new Jewelry { NameJewelry = "Кольцо из белого золота с рубином", IdJewelryTip = 1, PriceJewelry = 52000, IdMaterial = 3, IdStone = 2, IdSupplier = 1 },
                        new Jewelry { NameJewelry = "Серги из золота 'Гвоздика'", IdJewelryTip = 2, PriceJewelry = 28000, IdMaterial = 1, IdStone = 1, IdSupplier = 2 },
                        new Jewelry { NameJewelry = "Серги из серебра", IdJewelryTip = 2, PriceJewelry = 12000, IdMaterial = 2, IdStone = 4, IdSupplier = 2 },
                        new Jewelry { NameJewelry = "Серги из золота с сапфиром", IdJewelryTip = 2, PriceJewelry = 38000, IdMaterial = 1, IdStone = 3, IdSupplier = 2 },
                        new Jewelry { NameJewelry = "Ожерелье из белого золота", IdJewelryTip = 3, PriceJewelry = 35000, IdMaterial = 3, IdStone = 4, IdSupplier = 3 },
                        new Jewelry { NameJewelry = "Ожерелье с бриллиантом", IdJewelryTip = 3, PriceJewelry = 65000, IdMaterial = 2, IdStone = 1, IdSupplier = 3 },
                        new Jewelry { NameJewelry = "Цепочка из серебра", IdJewelryTip = 3, PriceJewelry = 10000, IdMaterial = 2, IdStone = 4, IdSupplier = 3 },
                        new Jewelry { NameJewelry = "Браслет из белого золото плетёный", IdJewelryTip = 4, PriceJewelry = 42000, IdMaterial = 3, IdStone = 3, IdSupplier = 1 },
                        new Jewelry { NameJewelry = "Браслет теннис с бриллиантом", IdJewelryTip = 4, PriceJewelry = 120000, IdMaterial = 3, IdStone = 1, IdSupplier = 2 }
                    );
                    context.SaveChanges();
                    Console.WriteLine("✅ Jewelries добавлены!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Ошибка Jewelries: {ex.Message}");
                }
            }
        }
    }
}