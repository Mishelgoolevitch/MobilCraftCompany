using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobilCraftCompany.Domain;

namespace MobilCraftCompany.Controllers.Admin
{
    [Authorize(Roles ="admin")]
    public partial class AdminController : Controller
    {
        private readonly DataManager _dataManager;
        public AdminController(DataManager dataManager)
        {
            _dataManager = dataManager;
        }
        public async Task<IActionResult> Index()
        { 
            ViewBag.ServiceCategoreies=await _dataManager.ServiceCategories.GetServiceCategoriesAsync();
            ViewBag.Services=await _dataManager.Services.GetServicesAsync();

            return View();
        }
    }
}