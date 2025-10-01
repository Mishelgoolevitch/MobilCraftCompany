using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobilCraftCompany.Domain;
using System.Text.Json;

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
            ViewBag.ServiceCategories=await _dataManager.ServiceCategories.GetServiceCategoriesAsync();
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
        //[HttpPost]
        //public async Task<JsonResult> SaveEditorImg()
        //{
        //    if (Request.Form.Files.Count == 0)
        //        return Json(new { error = "No file uploaded" });

        //    IFormFile img = Request.Form.Files[0];

        //    string folder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
        //    if (!Directory.Exists(folder))
        //        Directory.CreateDirectory(folder);

        //    string path = Path.Combine(folder, img.FileName);
        //    await using FileStream stream = new FileStream(path, FileMode.Create);
        //    await img.CopyToAsync(stream);

        //    return Json(new { location = $"/images/{img.FileName}" });
        //}


       // Сохраняем картинку из редактора
        public async Task<string> SaveEditorImg()
        {
            IFormFile img = Request.Form.Files[0];
            await SaveImg(img);

            return JsonSerializer.Serialize(new { location = Path.Combine("/images/", img.FileName) });
        }

    }
}