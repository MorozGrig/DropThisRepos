using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DropThisSite.Data;

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

        public IActionResult Index() => RedirectToAction(nameof(History));

        // GET: Orders/History (История заказов)
        public async Task<IActionResult> History()
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
