using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Services
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            //Convert request về dạng chuỗi json
            var json = JsonConvert.SerializeObject(request);

            //Add các header vào api
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient(); //tạo 1 đối tượng client

            //Địa chỉ đường dẫn mà ta muốn(ở đây là đường dẫn của prj BackendApi)
            //VD: client.BaseAddress = new Uri("https://localhost:5001");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            //post dữ liệu đến link nào đó(ở đây là link của phương thức authenticate trong prj BackendApi)
            var response = await client.PostAsync("/api/users/authenticate", httpContent);

            //Lấy ra token bằng cách lấy ra dạng string của response
            //var token = JsonConvert.DeserializeObject<ApiResult<string>>(await response.Content.ReadAsStringAsync());
            //return token;

            //29. do dùng ApiResult nên ta phải convert lại nó
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(await response.Content.ReadAsStringAsync());
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<string>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<PagedResult<UserVm>>> GetUsersPagings(GetUserPagingRequest request)
        {
            var client = _httpClientFactory.CreateClient(); //tạo 1 đối tượng client

            //Lấy token từ session thông qua HttpContextAccessor
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");

            //Địa chỉ đường dẫn mà ta muốn(ở đây là đường dẫn của prj BackendApi)
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            //Gán Header cho client
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            //post dữ liệu đến link nào đó(ở đây là link của phương thức authenticate trong prj BackendApi)
            var response = await client.GetAsync($"/api/users/paging?pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");

            //Lấy ra body bằng cách lấy ra dạng string của response
            var body = await response.Content.ReadAsStringAsync();

            //Chuyển ngược định dạng body đang là token về ban đầu
            var users = JsonConvert.DeserializeObject<ApiResult<PagedResult<UserVm>>>(body); //29

            //Deserialization là quá trình ngược lại của quá trình serialization,
            //thực hiện lấy dữ liệu từ các định dạng có cấu trúc, khôi phục thông tin theo byte, XML, JSON,... thành các đối tượng

            return users;
        }

        //28
        public async Task<ApiResult<bool>> RegisterUser(RegisterRequest registerRequest)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var json = JsonConvert.SerializeObject(registerRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/users", httpContent);
            //return response.IsSuccessStatusCode;

            //29
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        //29: Update User
        public async Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest request)
        {
            var client = _httpClientFactory.CreateClient();

            //Lấy token từ session thông qua HttpContextAccessor
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/users/{id}", httpContent);
            //return response.IsSuccessStatusCode;

            //29
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<UserVm>> GetById(Guid id)
        {
            var client = _httpClientFactory.CreateClient(); //tạo 1 đối tượng client

            //Lấy token từ session thông qua HttpContextAccessor
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");

            //Địa chỉ đường dẫn mà ta muốn(ở đây là đường dẫn của prj BackendApi)
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            //Gán Header cho client
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            //get dữ liệu từ link nào đó(ở đây là link của phương thức GetById trong prj BackendApi)
            var response = await client.GetAsync($"/api/users/{id}");

            //Lấy ra body bằng cách lấy ra dạng string của response
            var body = await response.Content.ReadAsStringAsync();

            //Chuyển ngược định dạng body đang là token về ban đầu
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<UserVm>>(body); //29

            //Deserialization là quá trình ngược lại của quá trình serialization,
            //thực hiện lấy dữ liệu từ các định dạng có cấu trúc, khôi phục thông tin theo byte, XML, JSON,... thành các đối tượng

            return JsonConvert.DeserializeObject<ApiErrorResult<UserVm>>(body); //29
        }
    }
}