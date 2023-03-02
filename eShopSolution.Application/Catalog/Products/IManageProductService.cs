using eShopSolution.Application.Catalog.Products.Dtos;
using eShopSolution.Application.Dtos;
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

        Task<int> Update(ProductEditRequest request);

        Task<int> Delete(int productId);

        Task<List<ProductViewModel>> GetAll(); //Lấy ra danh sách các thuộc tính mà ta muốn hiển thị

        Task<PagedViewModel<ProductViewModel>> GetAllPaging(string keyword, int pageIndex, int pageSize); //phân trang
    }
}
