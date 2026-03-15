using DropThisSite.Data;
using DropThisSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Diagnostics;

namespace DropThisSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index() => View();

        public IActionResult Privacy() => View();

        public IActionResult Cart()
        {
            var cart = GetCartFromSession();

            if (!cart.Any())
                return View(new List<Jewelry>());

            var cartItems = _context.Jewelries
                .Where(j => cart.Contains(j.IdJewelry))
                .ToList();

            ViewBag.TotalPrice = cartItems.Sum(j => (int)j.PriceJewelry);

            return View(cartItems);
        }

        [HttpGet]
        public IActionResult GetCartCount()
        {
            return Json(GetCartFromSession().Count);
        }

        [HttpPost]
        public IActionResult AddToCart(int id) // ✅ ПРОСТО int id
        {
            var cart = GetCartFromSession();
            if (!cart.Contains(id))
            {
                cart.Add(id);
                SetCartInSession(cart);
            }

            return Json(new { count = cart.Count });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // ✅ СЕССИЯ List<int>
        private List<int> GetCartFromSession()
        {
            var json = HttpContext.Session.GetString("Cart");
            return string.IsNullOrEmpty(json) ? new List<int>() :
                JsonSerializer.Deserialize<List<int>>(json) ?? new List<int>();
        }

        private void SetCartInSession(List<int> cart)
        {
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
        }


        [HttpPost]
        public IActionResult UpdateCartItem(int id, int quantity)
        {
            var cart = GetCartFromSession();

            cart.RemoveAll(x => x == id);
            for (int i = 0; i < quantity; i++)
                cart.Add(id);

            SetCartInSession(cart);

            // ✅ ТОЧНЫЙ ПОДСЧЁТ: цена × количество для ВСЕХ товаров
            decimal totalPrice = 0;
            var jewelryPrices = _context.Jewelries
                .Where(j => cart.Contains(j.IdJewelry))
                .ToDictionary(j => j.IdJewelry, j => j.PriceJewelry);

            foreach (var kvp in cart.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count()))
            {
                if (jewelryPrices.TryGetValue(kvp.Key, out int price))
                    totalPrice += price * kvp.Value;
            }

            return Json(new
            {
                totalPrice = totalPrice,
                totalItems = cart.Count
            });
        }


        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            var cart = GetCartFromSession();
            cart.RemoveAll(x => x == id);
            SetCartInSession(cart);

            TempData["Success"] = "Товар удален из корзины!";
            return RedirectToAction("Cart");
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = GetCartFromSession();
            if (cart.Count == 0)
                return RedirectToAction("Cart");

            ViewBag.CartIds = cart;
            ViewBag.TotalPrice = _context.Jewelries
                .Where(j => cart.Contains(j.IdJewelry))
                .Sum(j => j.PriceJewelry);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckoutProcess(string name, string phone, string address, string email)
        {
            var cart = GetCartFromSession();
            if (cart.Count == 0)
                return RedirectToAction("Index");

            var cartItems = await _context.Jewelries
                .Where(j => cart.Contains(j.IdJewelry))
                .ToListAsync();

            int totalPrice = cartItems.Sum(j => (int)j.PriceJewelry);

            // ✅ СОЗДАЁМ Order ДЛЯ КАЖДОГО ТОВАРА (твоя структура)
            foreach (var item in cartItems)
            {
                var order = new Order
                {
                    IdUser = int.TryParse(User.FindFirst("UserId")?.Value, out int userId) ? userId : 0,
                    IdJewelry = item.IdJewelry,
                    IdStatusOrder = 1,
                    OrderDate = DateTime.Now,
                    Quantity = 1,
                    TotalPrice = (int)item.PriceJewelry
                };

                _context.Orders.Add(order);
            }

            await _context.SaveChangesAsync();

            // Очищаем корзину
            HttpContext.Session.Remove("Cart");

            ViewBag.OrderCount = cartItems.Count;
            ViewBag.TotalPrice = totalPrice;
            return View();
        }

    }
}
