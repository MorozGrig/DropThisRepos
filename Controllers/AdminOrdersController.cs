using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DropThisSite.Data;
using DropThisSite.Models;

[Authorize(Roles = "Админ")]
public class AdminOrdersController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminOrdersController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var orders = _context.Orders
            .Include(o => o.User)
            .Include(o => o.StatusOrder)
            .OrderByDescending(o => o.OrderDate)
            .ToList();

        ViewBag.Statuses = _context.StatusOrders.ToList();
        return View(orders);
    }

    [HttpPost]
    public IActionResult UpdateStatus(int id, int statusId)
    {
        var order = _context.Orders.Find(id);
        if (order != null)
        {
            order.IdStatusOrder = statusId;
            _context.SaveChanges();
        }
        return Json(new { success = true });
    }
}