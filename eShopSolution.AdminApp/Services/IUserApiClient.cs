using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Services
{
    public interface IUserApiClient
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);

        //nên đặt trùng tên với phương thức trong mà ta cần tích hợp trong prj BackendApi

        Task<ApiResult<PagedResult<UserVm>>> GetUsersPagings(GetUserPagingRequest request);

        //28
        Task<ApiResult<bool>> RegisterUser(RegisterRequest registerRequest);

        //29. Update user
        Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest request);

        Task<ApiResult<UserVm>> GetById(Guid id);
    }
}