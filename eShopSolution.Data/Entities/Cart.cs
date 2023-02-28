using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public Guid UserId { get; set; }
        public Product Product { get; set; }
        public DateTime DateCreated { get; set; }
        //end 7

        public AppUser AppUser { get; set; }
        //end 11 
    }
}
