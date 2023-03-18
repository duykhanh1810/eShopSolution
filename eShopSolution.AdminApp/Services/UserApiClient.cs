using eShopSolution.ViewModels.System.Users;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Services
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> Authenticate(LoginRequest request)
        {
            //Convert request về dạng chuỗi json
            var json = JsonConvert.SerializeObject(request);

            //Add các header vào api
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient(); //tạo 1 đối tượng client

            //Địa chỉ đường dẫn mà ta muốn(ở đây là đường dẫn của prj BackendApi)
            client.BaseAddress = new Uri("https://localhost:5001");

            //post dữ liệu đến link nào đó(ở đây là link của phương thức authenticate trong prj BackendApi)
            var response = await client.PostAsync("/api/users/authenticate", httpContent);

            //Lấy ra token bằng cách lấy ra dạng string của response
            var token = await response.Content.ReadAsStringAsync();
            return token;
        }
    }
}