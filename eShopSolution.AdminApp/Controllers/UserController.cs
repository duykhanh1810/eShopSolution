using eShopSolution.AdminApp.Services;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;

        public UserController(IUserApiClient userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }

            //Lấy token từ request của Authenticate trong IUserApiClient
            var token = await _userApiClient.Authenticate(request);

            //26. Giải mã token

            // Gọi phương thức ValidateToken để chuyển token sang sang biến userPricipal
            var userPricipal = this.ValidateToken(token);

            //Xây dựng Authentication Properties(là 1 tập Properties của cookies, tham khảo từ link ở đoạn code được cmt)
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                //Thời gian mà Authentication hết hạn, khoảng thời gian này được gọi là vé xác thực(Authentication ticket)
                //(có thể hiểu nếu lâu k đăng nhập fb thì nó sẽ tự đăng xuất)

                IsPersistent = false,
                // Liệu phiên xác thực có được duy trì qua nhiều yêu cầu hay không.
                // Khi được sử dụng với cookie, kiểm soát xem thời gian tồn tại của cookie là tuyệt đối
                //	(khớp với thời gian tồn tại của vé xác thực) hay dựa trên phiên(Session).
            };

            //Gọi hàm SignInAsync của HttpContext
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPricipal,
                        authProperties);

            //Login thành công thì sẽ chuyển về trang chủ
            return RedirectToAction("Index", "Home");
        }

        //26. Hàm giải mã Token
        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }
    }
}