using eShopSolution.ApiIntegration;

using eShopSolution.ApiIntegration;

using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductApiClient _productApiClient;
		private readonly IConfiguration _configuration;

		private readonly ICategoryApiClient _categoryApiClient;

		public ProductController(IProductApiClient productApiClient,
			IConfiguration configuration,
			ICategoryApiClient categoryApiClient)
		{
			_configuration = configuration;
			_productApiClient = productApiClient;
			_categoryApiClient = categoryApiClient;
		}

		public async Task<IActionResult> Index(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 10)
		{
			var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);

			var request = new GetManageProductPagingRequest()
			{
				Keyword = keyword,
				PageIndex = pageIndex,
				PageSize = pageSize,
				LanguageId = languageId,
				CategoryId = categoryId
			};
			var data = await _productApiClient.GetPagings(request);
			ViewBag.Keyword = keyword;

			var categories = await _categoryApiClient.GetAll(languageId);
			ViewBag.Categories = categories.Select(x => new SelectListItem() //39. giá trị của ô dropdown filter
			{
				Text = x.Name,
				Value = x.Id.ToString(),
				Selected = categoryId.HasValue && categoryId.Value == x.Id
			});

			if (TempData["result"] != null)
			{
				ViewBag.SuccessMsg = TempData["result"];
			}
			return View(data);
		}

		//37. create product
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[Consumes("multipart/form-data")]
		public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
		{
			if (!ModelState.IsValid)
				return View();

			var result = await _productApiClient.CreateProduct(request);
			if (result)
			{
				TempData["result"] = "Create product successfully";
				return RedirectToAction("Index");
			}

			ModelState.AddModelError("", "Create product failed");
			return View(request);
		}

        //40. Assign Category:
        private async Task<CategoryAssignRequest> GetCategoryAssignRequest(int id)
		{
			var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);

			var productObj = await _productApiClient.GetById(id, languageId);
			var categories = await _categoryApiClient.GetAll(languageId);
			var categoryAssignRequest = new CategoryAssignRequest();
			foreach (var role in categories)
			{
				categoryAssignRequest.Categories.Add(new SelectItem()
				{
					Id = role.Id.ToString(),
					Name = role.Name,
					Selected = productObj.Categories.Contains(role.Name)
				});
			}
			return categoryAssignRequest;
		}

		[HttpGet]
		public async Task<IActionResult> CategoryAssign(int id)
		{
			var categoryAssignRequest = await GetCategoryAssignRequest(id);
			return View(categoryAssignRequest);
		}

		[HttpPost]
		public async Task<IActionResult> CategoryAssign(CategoryAssignRequest request)
		{
			if (!ModelState.IsValid)
				return View();

			var result = await _productApiClient.CategoryAssign(request.Id, request);
			if (result.IsSuccess)
			{
				TempData["result"] = "Category Assign for product successfully";
				return RedirectToAction("Index");
			}

			ModelState.AddModelError("", result.Message);
			var categoryAssignRequest = await GetCategoryAssignRequest(request.Id);
			return View(categoryAssignRequest);
		}

        //45. update product
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var product = await _productApiClient.GetById(id, languageId);
			var editVm = new ProductUpdateRequest()
			{
				Id = product.Id,
                Name = product.Name,
                Description = product.Description,
				Details = product.Details,
				SeoAlias = product.SeoAlias,
				SeoDescription = product.SeoDescription,
				SeoTitle = product.SeoTitle
			};
            return View(editVm);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _productApiClient.UpdateProduct(request);
            if (result)
            {
                TempData["result"] = "Update product successfully";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Update product failed");
            return View(request);
        }
    }
}