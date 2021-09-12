using Resturant.Web.Models;
using Resturant.Web.Services.Models;

namespace Resturant.Web.Services.IServices
{
    public interface IProductService : IBaseService
    {
        Task<ServiceResponse<T>> GetAllProductsAsync<T>(string token);
        Task<ServiceResponse<T>> GetAllProductsIdAsync<T>(int id,string token);
         Task<ServiceResponse<T>> CreateProductAsync<T>(ProductDto productDto,string token);
         Task<ServiceResponse<T>> UpdateProductAsync<T>(ProductDto productDto,string token);
         Task<ServiceResponse<T>> DeleteProductAsync<T>(int id,string token);
    }
}