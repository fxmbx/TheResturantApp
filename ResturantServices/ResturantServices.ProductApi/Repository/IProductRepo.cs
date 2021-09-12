using ResturantServices.ProductApi.Models;
using ResturantServices.ProductApi.Models.Dto;

namespace ResturantServices.ProductApi.Repository
{
    public interface IProductRepo
    {
         Task<ServiceResponse<IEnumerable<ProductDto>>> GetProducts();
         Task<ServiceResponse<ProductDto>> GetProductsById(int id);

         Task<ServiceResponse<ProductDto>> UpSertProduct(ProductDto productDto);

         Task<ServiceResponse<bool>> DeleteProdut(int id);
    }
}