using eShopSolution.Application.Catalog.Products.Dtos;
using eShopSolution.Application.Catalog.Products.Dtos.Manage;
using eShopSolution.Application.Dtos;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        private readonly EShopDbContext _context;

        public ManageProductService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product() //tạo 1 product mới
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                        SeoTitle = request.SeoTitle,
                        LanguageId = request.LanguageId
                    }
                }
                // Khai báo việc thêm request bằng việc gán Price trong Data bằng Price trong request
            };
            _context.Products.Add(product); //add product vào Db
            return await _context.SaveChangesAsync(); 
            //lưu vào db, dùng async để giảm thời gian phải chờ khi lưu dữ liệu vào db để làm việc khác
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            throw new NotImplementedException();
        }

        public async Task AddViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Can not find product: {productId}");
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<ProductViewModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request)
        {
            //1. Select Join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.ProductId equals c.Id
                        select new { p, pt, pic };

            //2. filter
            //trong trường hợp Keyword có dữ liệu - khác rỗng thì mới tìm
            if (!string.IsNullOrEmpty(request.Keyword))         
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));

            //nếu có bất cứ tìm kiếm nào liên quan đến sản phẩm trong list category
            if (request.CategoryIds.Count > 0)      
            {
                query = query.Where(x => request.CategoryIds.Contains(x.pic.CategoryId));
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
