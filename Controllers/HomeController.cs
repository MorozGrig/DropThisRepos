using DropThisSite.Data;
using DropThisSite.Models;
using DropThisSite.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;

namespace DropThisSite.Controllers
{
    public class HomeController : Controller
    {
        private const string InvalidAddressMessage = "Введите корректный адрес или выберите его из списка подсказок.";

        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var latestItems = _context.Jewelries
                .Include(j => j.JewelryTip)
                .Include(j => j.Material)
                .OrderByDescending(j => j.IdJewelry)
                .Take(3)
                .ToList();

            return View(latestItems);
        }

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
        public IActionResult AddToCart(int id)
        {
            var jewelryExists = _context.Jewelries.Any(j => j.IdJewelry == id);
            if (!jewelryExists)
            {
                if (IsAjaxRequest())
                {
                    return NotFound(new { error = "Товар не найден" });
                }

                TempData["Error"] = "Товар не найден";
                return RedirectToAction(nameof(Index));
            }

            var cart = GetCartFromSession();
            cart.Add(id);
            SetCartInSession(cart);

            if (IsAjaxRequest())
            {
                return Json(new { count = cart.Count });
            }

            TempData["Success"] = "Товар добавлен в корзину";
            return RedirectToAction(nameof(Cart));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

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

        private bool IsAjaxRequest()
        {
            return string.Equals(Request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.OrdinalIgnoreCase);
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
        public async Task<IActionResult> Checkout()
        {
            var cart = GetCartFromSession();
            if (cart.Count == 0)
                return RedirectToAction("Cart");

            ViewBag.CartIds = cart;
            SetCheckoutSummary(cart);
            SetYandexApiKey();

            var model = new CheckoutViewModel();

            if (int.TryParse(User.FindFirst("UserId")?.Value, out var userId) && userId > 0)
            {
                var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.IdUser == userId);
                if (user != null)
                {
                    model.Email = user.Email ?? string.Empty;
                    model.Phone = user.Phone ?? string.Empty;
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckoutProcess(CheckoutViewModel model)
        {
            var cart = GetCartFromSession();
            if (cart.Count == 0)
                return RedirectToAction("Index");

            ViewBag.CartIds = cart;
            SetCheckoutSummary(cart);
            SetYandexApiKey();

            if (!await IsAddressValidAsync(model))
            {
                ModelState.AddModelError(nameof(model.Address), InvalidAddressMessage);
            }

            if (!ModelState.IsValid)
            {
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


        private void SetYandexApiKey()
        {
            ViewBag.YandexApiKey = _configuration["YandexMaps:ApiKey"] ?? string.Empty;
        }

        private void SetCheckoutSummary(List<int> cart)
        {
            var groupedCart = cart
                .GroupBy(id => id)
                .ToDictionary(g => g.Key, g => g.Count());

            var prices = _context.Jewelries
                .Where(j => groupedCart.Keys.Contains(j.IdJewelry))
                .ToDictionary(j => j.IdJewelry, j => j.PriceJewelry);

            ViewBag.TotalPrice = groupedCart.Sum(i => prices.TryGetValue(i.Key, out var price) ? price * i.Value : 0);
        }

        private async Task<bool> IsAddressValidAsync(CheckoutViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Address))
            {
                return false;
            }

            if (model.IsAddressVerified)
            {
                return true;
            }

            var apiKey = _configuration["YandexMaps:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                _logger.LogWarning("YandexMaps API key is missing. Address verification fallback is disabled.");
                return false;
            }

            var encodedAddress = Uri.EscapeDataString(model.Address);
            var requestUri = $"https://geocode-maps.yandex.ru/1.x/?apikey={apiKey}&format=json&geocode={encodedAddress}";

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(requestUri);
                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                using var stream = await response.Content.ReadAsStreamAsync();
                using var doc = await JsonDocument.ParseAsync(stream);

                var foundCountText = doc.RootElement
                    .GetProperty("response")
                    .GetProperty("GeoObjectCollection")
                    .GetProperty("metaDataProperty")
                    .GetProperty("GeocoderResponseMetaData")
                    .GetProperty("found")
                    .GetString();

                return int.TryParse(foundCountText, out var foundCount) && foundCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to validate address via Yandex Geocoder API");
                return false;
            }
        }
    }
}
