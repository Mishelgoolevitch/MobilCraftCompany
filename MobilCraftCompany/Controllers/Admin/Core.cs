using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobilCraftCompany.Domain;

namespace MobilCraftCompany.Controllers.Admin
{
    [Authorize(Roles ="admin")]
    public partial class AdminController : Controller
    {
        private readonly DataManager _dataManager;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public AdminController(DataManager dataManager, IWebHostEnvironment hostingEnvironment)
        {
            _dataManager = dataManager;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> Index()
        { 
            ViewBag.ServiceCategoreies=await _dataManager.ServiceCategories.GetServiceCategoriesAsync();
            ViewBag.Services=await _dataManager.Services.GetServicesAsync();

            return View();
        }

        //Сохраняем картинку в файловую систему
        public async Task<string> SaveImg(IFormFile img)
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "images/", img.FileName);
            await using FileStream stream = new FileStream(path, FileMode.Create);
            await img.CopyToAsync(stream);

            return path;
        }
             
    }
}