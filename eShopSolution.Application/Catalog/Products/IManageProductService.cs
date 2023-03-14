using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    public interface IManageProductService
    {
        Task<int> Create(ProductCreateRequest request); //dùng thread để có thể xử lý nhiều request cùng 1 lúc
        //kiểu trả về là int tức là ta sẽ trả về mã sản phẩm mà chúng ta vừa tag

        Task<int> Update(ProductUpdateRequest request);

        Task<bool> UpdatePrice(int productId, decimal newPrice);

        Task<bool> UpdateStock(int productId, int addedQuantity);

        Task AddViewCount(int productId);

        Task<int> Delete(int productId);

        //Task<List<ProductViewModel>> GetAll(); //Lấy ra danh sách các thuộc tính mà ta muốn hiển thị  
        // thực tế ít dùng

        Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request); //phân trang

        //16 Image
        Task<int> AddImages(int productId, List<IFormFile> files);
        Task<int> RemoveImages(int productId);
        Task<int> UpdateImage(int imageId, string caption, bool isDefault);
        Task<List<ProductImageViewModel>> GetListImage(int productId);
    }
}
