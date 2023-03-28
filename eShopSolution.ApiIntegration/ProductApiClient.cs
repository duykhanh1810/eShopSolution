using eShopSolution.ApiIntegration;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
	public class ProductApiClient : BaseApiClient, IProductApiClient
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfiguration _configuration;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public ProductApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration,
			IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
		{
			_httpClientFactory = httpClientFactory;
			_configuration = configuration;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<PagedResult<ProductVm>> GetPagings(GetManageProductPagingRequest request)
		{
			var data = await GetAsync<PagedResult<ProductVm>>(
				$"/api/products/paging?pageIndex={request.PageIndex}" +
				$"&pageSize={request.PageSize}" +
				$"&keyword={request.Keyword}&languageId={request.LanguageId}&categoryId={request.CategoryId}");

			return data;
		}

		//37.Create product
		public async Task<bool> CreateProduct(ProductCreateRequest request)
		{
			var client = _httpClientFactory.CreateClient();

			var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);

			var languageId = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);

			client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

			var requestContent = new MultipartFormDataContent();

			//Chuyển Thumpnail image ra dạng binary content
			if (request.ThumbnailImage != null)
			{
				byte[] data;
				using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
				{
					data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
				}
				ByteArrayContent bytes = new ByteArrayContent(data);
				requestContent.Add(bytes, "thumbnails", request.ThumbnailImage.FileName);
			}

			//Add các kiểu dữ liệu còn lại vào theo dạng binary content
			//(ở ProductCreateRequest có bao nhiêu thì add hết vào)

			requestContent.Add(new StringContent(request.Price.ToString()), "price");
			requestContent.Add(new StringContent(request.OriginalPrice.ToString()), "originalPrice");
			requestContent.Add(new StringContent(request.Stock.ToString()), "stock");
			requestContent.Add(new StringContent(request.Name.ToString()), "name");
			requestContent.Add(new StringContent(request.Description.ToString()), "description");
			requestContent.Add(new StringContent(request.Details.ToString()), "details");
			requestContent.Add(new StringContent(request.SeoDescription.ToString()), "seoDescription");
			requestContent.Add(new StringContent(request.SeoTitle.ToString()), "seoTitle");
			requestContent.Add(new StringContent(request.SeoAlias.ToString()), "seoAlias");
			requestContent.Add(new StringContent(languageId), "languageId");

			//Post dữ liệu đi
			var response = await client.PostAsync($"/api/products", requestContent);
			return response.IsSuccessStatusCode;
		}

		//40.
		public async Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request)
		{
			var client = _httpClientFactory.CreateClient();
			client.BaseAddress = new Uri(_configuration["BaseAddress"]);
			var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

			var json = JsonConvert.SerializeObject(request);
			var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await client.PutAsync($"/api/products/{id}/categories", httpContent);
			var result = await response.Content.ReadAsStringAsync();
			if (response.IsSuccessStatusCode)
				return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);

			return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
		}

		public async Task<ProductVm> GetById(int id, string languageId)
		{
			var data = await GetAsync<ProductVm>($"/api/products/{id}/{languageId}");

			return data;
		}
	}
}