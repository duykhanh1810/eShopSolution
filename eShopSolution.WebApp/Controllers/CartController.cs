using eShopSolution.ApiIntegration;
using eShopSolution.Utilities.Constants;
using eShopSolution.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductApiClient _productApiClient;

        public CartController(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost("{id}/{languageId}")]
        public async Task<IActionResult> AddToCart(int id,string languageId)
        {
            var product = await _productApiClient.GetById(id,languageId); //lấy ra id product theo language

            //Kiểm tra session
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
            {
                //lấy ra cartSession            
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            }


            //check xem sản phẩm đã có trong giỏ hàng trước đó chưa
            int quantity = 1;
            if(currentCart.Any(x=>x.ProductId==id))
            {
                quantity = quantity + currentCart.First(x => x.ProductId == id).Quantity + 1;

                //nếu đã có sản phẩm đó trong giỏ hàng mà add thêm vào thì sẽ tự cộng thêm 1 trong quantity
            }

            var cartItem = new CartItemViewModel()
            {
                ProductId = id,
                Description = product.Description,
                Image = product.ThumbnailImage,
                Name = product.Name,
				Price = product.Price,
				Quantity = quantity
            };

            //if (currentCart == null) currentCart = new List<CartItemViewModel>();
            currentCart.Add(cartItem);

            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart); //trả về tổng current cart
        }

		[HttpGet]
		public IActionResult GetListItems()
		{
			var session = HttpContext.Session.GetString(SystemConstants.CartSession);
			List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
			if (session != null)
				currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
			return Ok(currentCart);
		}
	}
}
