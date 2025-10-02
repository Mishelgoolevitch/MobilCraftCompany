using Microsoft.AspNetCore.Mvc;
using MobilCraftCompany.Domain;
using MobilCraftCompany.Domain.Entities;
using MobilCraftCompany.Infrastructure;
using MobilCraftCompany.Models;

namespace MobilCraftCompany.Controllers
{
    public class ServicesController : Controller
    {
        private readonly DataManager _dataManager;

        public ServicesController(DataManager dataManager)
        {
            _dataManager = dataManager;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Service> list = await _dataManager.Services.GetServicesAsync();

            //Доменную сущность на клиенте использовать не рекомендуется, оборачиваем ее в DTO

            IEnumerable<ServiceDTO> listDTO = HelperDTO.TransformServices(list);

            return View(listDTO);

        }
    }
}
