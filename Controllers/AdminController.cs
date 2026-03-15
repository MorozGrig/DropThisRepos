using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DropThisSite.Data;

[Authorize(Roles = "Админ")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        ViewBag.JewelryCount = _context.Jewelries.Count();
        ViewBag.UserCount = _context.Users.Count();
        ViewBag.OrderCount = _context.Orders.Count();
        return View();
    }
}
