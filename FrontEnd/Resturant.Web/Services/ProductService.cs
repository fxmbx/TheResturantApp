using Resturant.Web.Models;
using Resturant.Web.Services.IServices;
using Resturant.Web.Services.Models;

namespace Resturant.Web.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IHttpClientFactory _clientFactory;
        public ProductService(IHttpClientFactory clientfactory) : base(clientfactory)
        {
            _clientFactory  =clientfactory;
        }

        public async Task<ServiceResponse<T>> CreateProductAsync<T>(ProductDto productDto,string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = productDto,
                Url= SD.ProductAPIBase +"api/products/AddProduct",
                Token = token
            });
        }

        public async Task<ServiceResponse<T>> DeleteProductAsync<T>(int id, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url= SD.ProductAPIBase +"api/products/"+ id,
                Token = token
            });
        }

        public async Task<ServiceResponse<T>> GetAllProductsAsync<T>(string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url= SD.ProductAPIBase +"api/products/",
                Token = token
            });
        }

        public async Task<ServiceResponse<T>> GetAllProductsIdAsync<T>(int id, string token)
        {
             return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url= SD.ProductAPIBase +"api/products/"+ id,
                Token = token
            });
        }

        public async Task<ServiceResponse<T>> UpdateProductAsync<T>(ProductDto productDto, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = productDto,
                Url= SD.ProductAPIBase +"api/products/EditProduct",
                Token = token
            });
        }
    }
}