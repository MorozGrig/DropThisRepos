using DropThisSite.Data;
using DropThisSite.Models;
using DropThisSite.Models.ViewModels;
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
            if (quantity < 1 || quantity > 100)
            {
                return BadRequest(new { error = "Количество должно быть от 1 до 100" });
            }

            var cart = GetCartFromSession();

            cart.RemoveAll(x => x == id);
            for (int i = 0; i < quantity; i++)
                cart.Add(id);

            SetCartInSession(cart);

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
            var groupedCart = cart
                .GroupBy(id => id)
                .ToDictionary(g => g.Key, g => g.Count());

            var prices = _context.Jewelries
                .Where(j => groupedCart.Keys.Contains(j.IdJewelry))
                .ToDictionary(j => j.IdJewelry, j => j.PriceJewelry);

            ViewBag.TotalPrice = groupedCart.Sum(i => prices.TryGetValue(i.Key, out var price) ? price * i.Value : 0);

            return View(new CheckoutViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckoutProcess(CheckoutViewModel model)
        {
            var cart = GetCartFromSession();
            if (cart.Count == 0)
                return RedirectToAction("Index");

            if (!ModelState.IsValid)
            {
                var grouped = cart.GroupBy(id => id).ToDictionary(g => g.Key, g => g.Count());
                var prices = _context.Jewelries
                    .Where(j => grouped.Keys.Contains(j.IdJewelry))
                    .ToDictionary(j => j.IdJewelry, j => j.PriceJewelry);

                ViewBag.TotalPrice = grouped.Sum(i => prices.TryGetValue(i.Key, out var price) ? price * i.Value : 0);
                return View("Checkout", model);
            }

            var groupedCart = cart
                .GroupBy(id => id)
                .ToDictionary(g => g.Key, g => g.Count());

            var cartItems = await _context.Jewelries
                .Where(j => groupedCart.Keys.Contains(j.IdJewelry))
                .ToListAsync();

            if (!cartItems.Any())
                return RedirectToAction("Cart");

            int totalQuantity = groupedCart.Sum(i => i.Value);
            int totalPrice = cartItems.Sum(item => item.PriceJewelry * groupedCart[item.IdJewelry]);

            int userId = int.TryParse(User.FindFirst("UserId")?.Value, out int parsedUserId) ? parsedUserId : 0;

            var order = new Order
            {
                IdUser = userId,
                IdJewelry = null,
                IdStatusOrder = 1,
                OrderDate = DateTime.Now,
                Quantity = totalQuantity,
                TotalPrice = totalPrice,
                CustomerName = model.Name,
                CustomerPhone = model.Phone,
                CustomerEmail = model.Email,
                DeliveryAddress = model.Address,
                OrderItems = cartItems.Select(item => new OrderItem
                {
                    IdJewelry = item.IdJewelry,
                    Quantity = groupedCart[item.IdJewelry],
                    UnitPrice = item.PriceJewelry,
                    TotalPrice = item.PriceJewelry * groupedCart[item.IdJewelry]
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("Cart");

            ViewBag.OrderCount = totalQuantity;
            ViewBag.TotalPrice = totalPrice;
            return View();
        }


    }
}
