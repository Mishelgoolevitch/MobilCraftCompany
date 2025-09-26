using Microsoft.AspNetCore.Mvc;

namespace MobilCraftCompany.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
