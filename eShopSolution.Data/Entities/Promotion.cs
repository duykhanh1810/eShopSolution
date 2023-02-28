using eShopSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Entities
{
    public class Promotion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool ApplyForAll { get; set; }
        public int? DiscountPercent { get; set; }
        public int? DiscountAmount { get; set; }
        public string ProductIds { get; set; }
        public string ProductCategoryIds { get; set; }
        public Status Status { get; set; }



    }
}
