using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Entities
{
    public class ProductInCategory
    {
        public int ProductId { get; set; } //Post
        public Product Product { get; set; } //thuộc tính liên kết - Tag
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
