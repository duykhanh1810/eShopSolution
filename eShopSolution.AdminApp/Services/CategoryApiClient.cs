using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Language;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Services
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {
        public CategoryApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<List<CategoryVm>> GetAll(string languageId)
        {
            return await GetListAsync<CategoryVm>("/api/categories?languageId=" + languageId);
        }
    }
}