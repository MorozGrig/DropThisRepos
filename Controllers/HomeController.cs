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

        // ✅ КОРЗИНА (только ID товаров)
        public IActionResult Cart()
        {
            var cartIds = GetCartFromSession();
            var jewelry = _context.Jewelries
                .Where(j => cartIds.Contains(j.IdJewelry))
                .ToList();
            ViewBag.CartCount = cartIds.Count;
            return View(jewelry);
        }

        [HttpGet]
        public IActionResult GetCartCount()
        {
            return Json(GetCartFromSession().Count);
        }

        [HttpPost]
        [HttpPost]
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


        [HttpPost]
        public IActionResult RemoveFromCart(int jewelryId)
        {
            var cart = GetCartFromSession();
            cart.Remove(jewelryId);
            SetCartInSession(cart);
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
    }
}
