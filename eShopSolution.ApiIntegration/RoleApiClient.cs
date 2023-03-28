using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Roles;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
	public class RoleApiClient : IRoleApiClient
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfiguration _configuration;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public RoleApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration,
			IHttpContextAccessor httpContextAccessor)
		{
			_httpClientFactory = httpClientFactory;
			_configuration = configuration;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<ApiResult<List<RoleVm>>> GetAll()
		{
			var client = _httpClientFactory.CreateClient(); //tạo 1 đối tượng client

			//Lấy token từ session thông qua HttpContextAccessor
			var session = _httpContextAccessor.HttpContext.Session.GetString("Token");

			//Địa chỉ đường dẫn mà ta muốn(ở đây là đường dẫn của prj BackendApi)
			client.BaseAddress = new Uri(_configuration["BaseAddress"]);

			//Gán Header cho client
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

			//get dữ liệu từ link nào đó(ở đây là link của phương thức GetById trong prj BackendApi)
			var response = await client.GetAsync($"/api/roles");

			//Lấy ra body bằng cách lấy ra dạng string của response
			var body = await response.Content.ReadAsStringAsync();

			//Chuyển ngược định dạng body đang là token về ban đầu
			if (response.IsSuccessStatusCode)
			{
				List<RoleVm> myDeserializedObjList = (List<RoleVm>)JsonConvert.DeserializeObject(body, typeof(List<RoleVm>));
				return new ApiSuccessResult<List<RoleVm>>(myDeserializedObjList);
			}

			//Deserialization là quá trình ngược lại của quá trình serialization,
			//thực hiện lấy dữ liệu từ các định dạng có cấu trúc, khôi phục thông tin theo byte, XML, JSON,... thành các đối tượng

			return JsonConvert.DeserializeObject<ApiErrorResult<List<RoleVm>>>(body); //29
		}
	}
}