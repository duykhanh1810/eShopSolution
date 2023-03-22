using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
        }

        public async Task<ApiResult<string>> Authencate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName); //tìm username
            if (user == null) return null;

            //check pass
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            //username, password,rememberMe, khi mà login failed nhiều quá thì sẽ khóa tài khoản = true

            if (!result.Succeeded) { return null; }

            //đăng nhập thành công
            var roles = await _userManager.GetRolesAsync(user); //lấy danh sách role, trả về list string
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.Role, string.Join(";",roles)), // đổi 1 danh sách role sang chuỗi
                new Claim(ClaimTypes.Name, request.UserName)
            };

            //Mã hóa claims
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new ApiSuccessResult<string>(new JwtSecurityTokenHandler().WriteToken(token)); //đăng nhập thành công thì trả về 1 token
        }

        public async Task<ApiResult<UserVm>> GetById(Guid id) //29. get by id for update user
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<UserVm>("User không tồn tại"); // chú ý phải trùng kiểu dữ liệu của phương thức
            }
            var uservm = new UserVm()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Dob = user.Dob,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
            return new ApiSuccessResult<UserVm>(uservm);
        }

        public async Task<ApiResult<PagedResult<UserVm>>> GetUsersPaging(GetUserPagingRequest request) //29
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(request.Keyword)
                || x.PhoneNumber.Contains(request.Keyword));
            }

            int totalRow = await query.CountAsync(); // biến lấy tổng số bản ghi hiện tại sau khi search

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new UserVm()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserName = x.UserName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                }).ToListAsync();
            //Có thể hiểu như sau: ở trang đầu có 10 bản ghi thì sẽ là(1 - 1) * 10 thì số bản ghi bị bỏ qua sẽ là 0,
            //tức là take 10 bản ghi đầu ở trang 1
            // trang 2 sẽ là(2 - 1)*10 = 10, bỏ qua 10 bản ghi đầu tiên thì sẽ đến trang số 2

            //4. Select and Projection
            var pagedResult = new PagedResult<UserVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<UserVm>>(pagedResult);
        }

        public async Task<ApiResult<bool>> Register(RegisterRequest request)
        {
            var check = await _userManager.FindByNameAsync(request.UserName);
            if (check != null)
            {
                return new ApiErrorResult<bool>("Username đã tồn tại");
            }
            if (await _userManager.FindByNameAsync(request.Email) != null)
            {
                return new ApiErrorResult<bool>("Email đã tồn tại");
            }

            var user = new AppUser()
            {
                Dob = request.Dob,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Đăng ký không thành công");
        }

        //29: update User
        public async Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != id)) //check email
            {
                return new ApiErrorResult<bool>("Email đã tồn tại");
            }

            var user = await _userManager.FindByIdAsync(id.ToString()); //lấy ra user

            //Update theo những thuộc tính đã khai báo trong UserUpdateRequest
            user.Dob = request.Dob;
            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Cập nhật không thành công");
        }

        //31. delete user
        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("User không tồn tại"); // chú ý phải trùng kiểu dữ liệu của phương thức
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return new ApiSuccessResult<bool>();

            return new ApiErrorResult<bool>("Xóa không thành công");
        }
    }
}