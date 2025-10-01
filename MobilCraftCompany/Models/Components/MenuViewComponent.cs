using Microsoft.AspNetCore.Mvc;
using MobilCraftCompany.Domain;
using MobilCraftCompany.Domain.Entities;
using MobilCraftCompany.Infrastructure;

namespace MobilCraftCompany.Models.Components
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly DataManager _dataManager;

            public  MenuViewComponent(DataManager dataManager)
        {
            _dataManager= dataManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<Service> list=await _dataManager.Services.GetServicesAsync();

            //Доменную сущность на клиенте использовать не рекомендуется, оборачиваем ее в DTO

            IEnumerable<ServiceDTO> listDTO=HelperDTO.TransformServices(list);

            return await Task.FromResult((IViewComponentResult) View("Defalt", listDTO));
        }
    }
}
