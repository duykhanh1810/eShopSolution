using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Entities
{
	//7
	public class Product
	{
		public int Id { get; set; }
		public decimal Price { get; set; }
		public decimal OriginalPrice { get; set; }
		public int Stock { get; set; }
		public int ViewCount { get; set; }
		public DateTime DateCreated { get; set; }
		//end 7

		public bool? IsFeatured { get; set; } //43

		//8
		public List<ProductInCategory> ProductInCategories { get; set; }

		public List<OrderDetail> OrderDetails { get; set; }

		public List<Cart> Carts { get; set; }

		public List<ProductTranslation> ProductTranslations { get; set; }

		//15
		public List<ProductImage> ProductImages { get; set; }
	}
}