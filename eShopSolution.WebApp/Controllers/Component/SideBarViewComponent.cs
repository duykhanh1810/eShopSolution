using eShopSolution.ApiIntegration;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Controllers.Component
{
    public class SideBarViewComponent : ViewComponent
    {
        private readonly ICategoryApiClient _categoryApiClient;

        public SideBarViewComponent(ICategoryApiClient categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //lấy ra danh sách category
            var items = await _categoryApiClient.GetAll(CultureInfo.CurrentCulture.Name);
            return View(items);
        }
    }
}