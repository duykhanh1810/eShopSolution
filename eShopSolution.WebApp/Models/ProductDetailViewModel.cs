using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Catalog.ProductImage;
using eShopSolution.ViewModels.Catalog.Products;
using System.Collections.Generic;

namespace eShopSolution.WebApp.Models
{
	public class ProductDetailViewModel
	{
        public CategoryVm Category { get; set; }
        public ProductVm Product { get; set; }
        public List<ProductVm> RelatedProduct { get; set; }
        public List<ProductImageViewModel> ProductImages { get; set; }
    }
}
