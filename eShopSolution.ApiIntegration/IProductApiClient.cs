using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using System;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
	public interface IProductApiClient
	{
		Task<PagedResult<ProductVm>> GetPagings(GetManageProductPagingRequest request);

		//37
		Task<bool> CreateProduct(ProductCreateRequest request);

		//40.
		Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);

		Task<ProductVm> GetById(int id, string languageId);
	}
}