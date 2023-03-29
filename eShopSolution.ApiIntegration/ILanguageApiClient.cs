using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Languages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
	public interface ILanguageApiClient
	{
		Task<ApiResult<List<LanguageVm>>> GetAll();
	}
}