using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Language;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using eShopSolution.Utilities.Constants;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Services
{
    public class BaseApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        protected async Task<TResponse> GetAsync<TResponse>(string url)
        {
            var client = _httpClientFactory.CreateClient(); //tạo 1 đối tượng client

            //Lấy token từ session thông qua HttpContextAccessor
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);

            //Địa chỉ đường dẫn mà ta muốn(ở đây là đường dẫn của prj BackendApi)
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);

            //Gán Header cho client
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            //get dữ liệu từ link nào đó(ở đây là link của phương thức GetById trong prj BackendApi)
            var response = await client.GetAsync(url);

            //Lấy ra body bằng cách lấy ra dạng string của response
            var body = await response.Content.ReadAsStringAsync();

            //Chuyển ngược định dạng body đang là token về ban đầu
            if (response.IsSuccessStatusCode)
            {
                TResponse myDeserializedObjList = (TResponse)JsonConvert.DeserializeObject(body, typeof(TResponse));
                return myDeserializedObjList;
            }

            //Deserialization là quá trình ngược lại của quá trình serialization,
            //thực hiện lấy dữ liệu từ các định dạng có cấu trúc, khôi phục thông tin theo byte, XML, JSON,... thành các đối tượng

            return JsonConvert.DeserializeObject<TResponse>(body); //29
        }

        public async Task<List<T>> GetListAsync<T>(string url, bool requiredLogin = false)
        {
            var sessions = _httpContextAccessor
               .HttpContext
               .Session
               .GetString(SystemConstants.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var data = (List<T>)JsonConvert.DeserializeObject(body, typeof(List<T>));
                return data;
            }
            throw new Exception(body);
        }
    }
}