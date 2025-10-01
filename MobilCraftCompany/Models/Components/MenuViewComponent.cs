using Microsoft.AspNetCore.Mvc;
using MobilCraftCompany.Domain;
using MobilCraftCompany.Domain.Entities;

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
            return await Task.FromResult((IViewComponentResult) View("Defalt", list));
        }
    }
}
