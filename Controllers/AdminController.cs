using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace DropThisSite.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private const int AdminRoleId = 2;
        private const string ExternalAdminApplicationPath = @"C:\DropThis\AdminPanel\DropThisAdmin.exe";

        public IActionResult Launch()
        {
            if (!IsCurrentUserAdmin())
                return Forbid();

            if (!System.IO.File.Exists(ExternalAdminApplicationPath))
            {
                TempData["AdminLaunchError"] = $"Не удалось открыть админку: файл внешнего приложения не найден по пути {ExternalAdminApplicationPath}.";
                return RedirectToPreviousPage();
            }

            StartExternalAdminApplication();
            return RedirectToPreviousPage();
        }

        private bool IsCurrentUserAdmin()
        {
            return int.TryParse(User.FindFirst("IdRole")?.Value, out var idRole) && idRole == AdminRoleId;
        }

        private static void StartExternalAdminApplication()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = ExternalAdminApplicationPath,
                UseShellExecute = true
            });
        }

        private IActionResult RedirectToPreviousPage()
        {
            var referer = Request.Headers.Referer.ToString();
            if (!string.IsNullOrWhiteSpace(referer))
                return Redirect(referer);

            return RedirectToAction("Index", "Home");
        }
    }
}
