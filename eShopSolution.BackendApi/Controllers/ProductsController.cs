using eShopSolution.Application.Catalog.Products;
using eShopSolution.ViewModels.Catalog.ProductImage;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        //http://localhost:port/product
        //[HttpGet("{languageId}")]
        //public async Task<IActionResult> GetAll(string languageId)
        //{
        //    var products = await _publicProductService.GetAll(languageId); ;
        //    return Ok(products);
        //}

        //19. RESTful Api for product

        //http://localhost:port/products?pageIndex=1&pageSize=10&CategoryId=
        //[HttpGet("{languageId}")]
        //public async Task<IActionResult> GetAllPaging(string languageId, [FromQuery] GetPublicProductPagingRequest request)
        //{
        //    var products = await _productService.GetAllByCategoryId(languageId, request);
        //    return Ok(products);
        //}

        //http://localhost:port/product/public-paging/1
        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var products = await _productService.GetById(productId, languageId);
            if (products == null)
                return BadRequest("Cannot find product");
            return Ok(products);
        }

        //Create
        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productId = await _productService.Create(request);
            if (productId == 0)
                return BadRequest();

            var product = await _productService.GetById(productId, request.LanguageId);

            //return Created(nameof(GetById),product); //c1 trả về status 200
            return CreatedAtAction(nameof(GetById), new { id = productId }, product); //trả về status 201

            //nameof tạo ra tên của một biến, loại hoặc thành viên dưới dạng hằng chuỗi(ở đây trả về GetById)
        }

        //Update
        [HttpPut("{productId}")]
        [Consumes("multipart/form-data")]
		[Authorize]
		public async Task<IActionResult> Update([FromRoute] int productId,[FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.Id = productId;
            var affectedResult = await _productService.Update(request);
            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }

        //Delete
        [HttpDelete("productId")]
		[Authorize]
		public async Task<IActionResult> Delete(int productId)
        {
            var affectedResult = await _productService.Delete(productId);
            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }

        //UpdatePrice
        [HttpPatch("{productId}/{newPrice}")]
		[Authorize]
		public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            var isSuccessful = await _productService.UpdatePrice(productId, newPrice);
            if (isSuccessful)
                return Ok();

            return BadRequest();
        }

        //20.RESTful Api for image

        [HttpGet("{productId}/images/{imageId}")] //chú ý phải qua images thì mới đến imageId
        public async Task<IActionResult> GetImageById(int productId, int imageId)
        {
            var image = await _productService.GetImageById(imageId);
            if (image == null)
                return BadRequest("Cannot find image");
            return Ok(image);
        }

        [HttpPost("{productId}/images")]
		[Authorize]
		public async Task<IActionResult> CreateImage(int productId, [FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var imageId = await _productService.AddImages(productId, request);
            if (imageId == 0)
                return BadRequest();

            var image = await _productService.GetImageById(imageId);

            //return Created(nameof(GetById),product); //c1 trả về status 200
            return CreatedAtAction(nameof(GetById), new { id = imageId }, image); //trả về status 201
        }

        [HttpPut("{productId}/images/{imageId}")]
		[Authorize]
		public async Task<IActionResult> UpdateImage(int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productService.UpdateImage(imageId, request);
            if (result == 0)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("{productId}/images/{imageId}")]
		[Authorize]
		public async Task<IActionResult> RemoveImage(int imageId)
        {
            var affectedResult = await _productService.RemoveImages(imageId);
            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }

        //36. Product list
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetManageProductPagingRequest request)
        {
            var products = await _productService.GetAllPaging(request);
            return Ok(products);
        }

        //40. category assign
        [HttpPut("{id}/categories")]
		[Authorize]
		public async Task<IActionResult> CategoryAssign(int id, [FromBody] CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.CategoryAssign(id, request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        //43. get feature product
        [HttpGet("featured/{languageId}/{take}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFeatureProducts(string languageId, int take)
        {
            var products = await _productService.GetFeatureProduct(languageId, take);
            return Ok(products);
        }

        //44. get latest product
        [HttpGet("latest/{languageId}/{take}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLatestProducts(string languageId, int take)
        {
            var products = await _productService.GetLatestProduct(languageId, take);
            return Ok(products);
        }
    }
}