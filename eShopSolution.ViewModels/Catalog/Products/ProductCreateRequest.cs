using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Catalog.Products
{
	public class ProductCreateRequest
	{
		public decimal Price { get; set; }
		public decimal OriginalPrice { get; set; }
		public int Stock { get; set; }
		public string Name { set; get; }
		public string Description { set; get; }
		public string Details { set; get; }
		public string SeoDescription { set; get; }
		public string SeoTitle { set; get; }

		public string SeoAlias { get; set; }
		public string LanguageId { set; get; }

		//public Product Product { get; set; }

		//public Language Language { get; set; }

		public bool? IsFeatured { get; set; } //43

		public IFormFile ThumbnailImage { get; set; }
	}
}