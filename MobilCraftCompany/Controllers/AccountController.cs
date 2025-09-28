using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MobilCraftCompany.Models;

namespace MobilCraftCompany.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl)
        {
            await _signInManager.SignOutAsync();
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
          await  _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
