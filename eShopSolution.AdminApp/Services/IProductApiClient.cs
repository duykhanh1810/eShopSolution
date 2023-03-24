using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Services
{
    public interface IProductApiClient
    {
        Task<PagedResult<ProductVm>> GetPagings(GetManageProductPagingRequest request);

        //37
        Task<bool> CreateProduct(ProductCreateRequest request);
    }
}