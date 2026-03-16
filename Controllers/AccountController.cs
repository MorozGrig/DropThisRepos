using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DropThisSite.Data;
using DropThisSite.Models;
using DropThisSite.Models.ViewModels;
using System.Security.Claims;

namespace DropThisSite.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login() => View(new LoginViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Login ?? string.Empty),
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                    new Claim("UserId", user.IdUser.ToString())
                };

                if (user.Role != null)
                    claims.Add(new Claim(ClaimTypes.Role, user.Role.NameRole ?? "Пользователь"));

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Неверный логин или пароль";
            return View(model);
        }

        public IActionResult Register() => View(new RegisterViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (_context.Users.Any(u => u.Login == model.Login || u.Email == model.Email))
            {
                ViewBag.Error = "Логин или адрес электронной почты уже используется";
                return View(model);
            }

            var user = new User
            {
                Login = model.Login,
                Email = model.Email,
                Phone = model.Phone,
                Password = model.Password,
                IdRole = 2
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await Login(new LoginViewModel { Login = model.Login, Password = model.Password });
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            if (!int.TryParse(User.FindFirst("UserId")?.Value, out var userId) || userId <= 0)
                return Challenge();

            var user = await _context.Users
                .Include(u => u.Orders!)
                    .ThenInclude(o => o.StatusOrder)
                .Include(u => u.Orders!)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(oi => oi.Jewelry)
                .FirstOrDefaultAsync(u => u.IdUser == userId);

            if (user == null)
                return NotFound();

            var model = new ProfileViewModel
            {
                Login = user.Login ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Phone = user.Phone ?? string.Empty,
                Orders = user.Orders?.OrderByDescending(o => o.OrderDate).ToList() ?? new List<Order>()
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!int.TryParse(User.FindFirst("UserId")?.Value, out var userId) || userId <= 0)
                return Challenge();

            var user = await _context.Users
                .Include(u => u.Orders!)
                    .ThenInclude(o => o.StatusOrder)
                .Include(u => u.Orders!)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(oi => oi.Jewelry)
                .FirstOrDefaultAsync(u => u.IdUser == userId);

            if (user == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                model.Orders = user.Orders?.OrderByDescending(o => o.OrderDate).ToList() ?? new List<Order>();
                return View(model);
            }

            user.Login = model.Login;
            user.Email = model.Email;
            user.Phone = model.Phone;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Профиль успешно обновлен";
            return RedirectToAction(nameof(Profile));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
