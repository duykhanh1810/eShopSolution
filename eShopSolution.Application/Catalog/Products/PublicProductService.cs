using eShopSolution.Application.Common;
using eShopSolution.Data.EF;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    public class PublicProductService : IPublicProductService
    {
        private readonly EShopDbContext _context;
        public PublicProductService(EShopDbContext context)
        {
            _context = context;
        }

        //public async Task<List<ProductViewModel>> GetAll(string languageId)
        //{
        //    var query = from p in _context.Products
        //                join pt in _context.ProductTranslations on p.Id equals pt.ProductId
        //                join pic in _context.ProductInCategories on p.Id equals pic.ProductId
        //                join c in _context.Categories on pic.ProductId equals c.Id
        //                where pt.LanguageId == languageId //19
        //                select new { p, pt, pic };


        //    //3.Get product

        //    var data = await query.Select(x => new ProductViewModel()
        //        {
        //            Id = x.p.Id,
        //            Name = x.pt.Name,
        //            DateCreated = x.p.DateCreated,
        //            Description = x.pt.Description,
        //            Details = x.pt.Details,
        //            LanguageId = x.pt.LanguageId,
        //            OriginalPrice = x.p.OriginalPrice,
        //            Price = x.p.Price,
        //            SeoAlias = x.pt.SeoAlias,
        //            SeoDescription = x.pt.SeoDescription,
        //            SeoTitle = x.pt.SeoTitle,
        //            Stock = x.p.Stock,
        //            ViewCount = x.p.ViewCount
        //        }).ToListAsync();
        //    return data;
        //}

        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(string languageId,GetPublicProductPagingRequest request)
        {
            //1. Select Join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.ProductId equals c.Id
                        where pt.LanguageId == languageId //19
                        select new { p, pt, pic };

            //2. filter

            //nếu có bất cứ tìm kiếm nào liên quan đến sản phẩm trong list category
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(p=>p.pic.CategoryId == request.CategoryId);
            }

            //3.Paging
            int totalRow = await query.CountAsync(); // biến lấy tổng số bản ghi hiện tại sau khi search

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();
            //Có thể hiểu như sau: ở trang đầu có 10 bản ghi thì sẽ là(1 - 1) * 10 thì số bản ghi bị bỏ qua sẽ là 0,
            //tức là take 10 bản ghi đầu ở trang 1
            // trang 2 sẽ là(2 - 1)*10 = 10, bỏ qua 10 bản ghi đầu tiên thì sẽ đến trang số 2


            //4. Select and Projection
            var pagedResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data
            };
            return pagedResult;
        }
    }
}
