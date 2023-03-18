﻿using eShopSolution.ViewModels.System.Users;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Services
{
    public interface IUserApiClient
    {
        Task<string> Authenticate(LoginRequest request);

        //nên đặt trùng tên với phương thức trong mà ta cần tích hợp trong prj BackendApi
    }
}