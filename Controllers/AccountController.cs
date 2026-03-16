using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
                    claims.Add(new Claim(ClaimTypes.Role, user.Role.NameRole ?? "User"));

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
                ViewBag.Error = "Логин или email занят";
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

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
