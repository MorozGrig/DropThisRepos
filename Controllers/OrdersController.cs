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
            if (!int.TryParse(User.FindFirst("UserId")?.Value, out var userId) || userId <= 0)
            {
                return Challenge();
            }

            var orders = await _context.Orders
                                .Include(o => o.StatusOrder)
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Jewelry)
                        .ThenInclude(j => j.Material)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Jewelry)
                        .ThenInclude(j => j.Stone)
                .Where(o => o.IdUser == userId)
                .OrderByDescending(o => o.OrderDate)
                .AsSplitQuery()
                .ToListAsync();

            return View(orders);
        }
    }
}