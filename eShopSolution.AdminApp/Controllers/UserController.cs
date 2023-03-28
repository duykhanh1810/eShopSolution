using eShopSolution.ApiIntegration;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
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
	public class UserController : BaseController
	{
		private readonly IUserApiClient _userApiClient;
		private readonly IConfiguration _configuration;
		private readonly IRoleApiClient _roleApiClient;

		public UserController(IUserApiClient userApiClient, IConfiguration configuration, IRoleApiClient roleApiClient)
		{
			_userApiClient = userApiClient;
			_configuration = configuration;
			_roleApiClient = roleApiClient;
		}

		public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
		{
			var request = new GetUserPagingRequest()
			{
				Keyword = keyword,
				PageIndex = pageIndex,
				PageSize = pageSize
			};
			var data = await _userApiClient.GetUsersPagings(request);
			ViewBag.Keyword = keyword;
			if (TempData["result"] != null)
			{
				ViewBag.SuccessMsg = TempData["result"];
			}
			return View(data.ResultObject);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(RegisterRequest request)
		{
			if (!ModelState.IsValid)
				return View();

			var result = await _userApiClient.RegisterUser(request);
			if (result.IsSuccess)
			{
				TempData["result"] = "Create user successfully";
				return RedirectToAction("Index");
			}

			ModelState.AddModelError("", result.Message);
			return View(request);
		}

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			HttpContext.Session.Remove("Token");
			return RedirectToAction("Index", "Login");
		}

		//29. Update user
		[HttpGet]
		public async Task<IActionResult> Edit(Guid id)
		{
			var result = await _userApiClient.GetById(id);
			if (result.IsSuccess)
			{
				var user = result.ResultObject;
				var update = new UserUpdateRequest()
				{
					Id = user.Id,
					FirstName = user.FirstName,
					LastName = user.LastName,
					Dob = user.Dob,
					Email = user.Email,
					PhoneNumber = user.PhoneNumber
				};
				return View(update);
			}
			return RedirectToAction("Error", "Home");
		}

		[HttpPost]
		public async Task<IActionResult> Edit(UserUpdateRequest request)
		{
			if (!ModelState.IsValid)
				return View();

			var result = await _userApiClient.UpdateUser(request.Id, request);
			if (result.IsSuccess)
			{
				TempData["result"] = "Edit user successfully";
				return RedirectToAction("Index");
			}

			ModelState.AddModelError("", result.Message);
			return View(request);
		}

		//30. PagedList
		[HttpGet]
		public async Task<IActionResult> Details(Guid id)
		{
			var result = await _userApiClient.GetById(id);
			return View(result.ResultObject);
		}

		//31. Delete user

		[HttpGet]
		public IActionResult Delete(Guid id)
		{
			return View(new UserDeleteRequest()
			{
				Id = id
			});
		}

		[HttpPost]
		public async Task<IActionResult> Delete(UserDeleteRequest request)
		{
			if (!ModelState.IsValid)
				return View();

			var result = await _userApiClient.Delete(request.Id);
			if (result.IsSuccess)
			{
				TempData["result"] = "Delete user successfully";
				return RedirectToAction("Index");
			}

			ModelState.AddModelError("", result.Message);
			return View(request);
		}

		//34.Role Assign

		private async Task<RoleAssignRequest> GetRoleAssignRequest(Guid id)
		{
			var userObj = await _userApiClient.GetById(id);
			var roleObj = await _roleApiClient.GetAll();
			var roleAssignRequest = new RoleAssignRequest();
			foreach (var role in roleObj.ResultObject)
			{
				roleAssignRequest.Roles.Add(new SelectItem()
				{
					Id = role.Id.ToString(),
					Name = role.Name,
					Selected = userObj.ResultObject.Roles.Contains(role.Name)
					//nếu Contains role.Name thì Selected sẽ bằng true, không thì sẽ là failed
				});
			}
			return roleAssignRequest;
		}

		[HttpGet]
		public async Task<IActionResult> RoleAssign(Guid id)
		{
			var roleAssignRequest = await GetRoleAssignRequest(id);
			return View(roleAssignRequest);
		}

		[HttpPost]
		public async Task<IActionResult> RoleAssign(RoleAssignRequest request)
		{
			if (!ModelState.IsValid)
				return View();

			var result = await _userApiClient.RoleAssign(request.Id, request);
			if (result.IsSuccess)
			{
				TempData["result"] = "Role Assign for user successfully";
				return RedirectToAction("Index");
			}

			ModelState.AddModelError("", result.Message);
			var roleAssignRequest = await GetRoleAssignRequest(request.Id);
			return View(roleAssignRequest);
		}
	}
}