using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DropThisSite.Data;
using DropThisSite.Models;

namespace DropThisSite.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders/Index (Мои заказы)
        public async Task<IActionResult> Index()
        {
            int userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

            var orders = await _context.Orders
                .Include(o => o.Jewelry)
                .Include(o => o.StatusOrder)
                .Include(o => o.User)
                .Where(o => o.IdUser == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }
    }
}