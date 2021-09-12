using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ResturantServices.ProductApi.Data;
using ResturantServices.ProductApi.Models;
using ResturantServices.ProductApi.Models.Dto;
using System.Linq;

namespace ResturantServices.ProductApi.Repository
{
    public class ProductRepo : IProductRepo
    {
        private readonly ProductDbContext dbContext;
        private IMapper mapper;
        public ProductRepo(ProductDbContext _dbcontext, IMapper _mapper)
        {
            dbContext = _dbcontext;
            mapper =_mapper;
        }
        public async Task<ServiceResponse<bool>> DeleteProdut(int id)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();
            try
            {
                var product = await dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == id);
                if(product ==null)
                {
                    response.Data = false;
                    response.Success=false;
                    response.Message = "Product not found";
                    return response;
                }
                 dbContext.Products.Remove(product);
                 await dbContext.SaveChangesAsync(true);
            }catch(Exception ex)
            {
                response.ErrorMessages = new List<string>{ex.Message};
                response.Success=false;
            }
            return response;
        }

        public async Task<ServiceResponse<IEnumerable<ProductDto>>> GetProducts()
        {
            ServiceResponse<IEnumerable<ProductDto>> response = new ServiceResponse<IEnumerable<ProductDto>>();
            try
            {
                var products = await dbContext.Products.ToListAsync();
                response.Data = mapper.Map<IEnumerable<ProductDto>>(products);
            }catch(Exception ex)
            {
               response.ErrorMessages = new List<string>{ex.Message};
               response.Success = false;
            }
            return response;
        }

        public async Task<ServiceResponse<ProductDto>> GetProductsById(int id)
        {
            
            ServiceResponse<ProductDto> response = new ServiceResponse<ProductDto>();
            try
            {
                var productbyId = await dbContext.Products.FirstOrDefaultAsync(x=>x.ProductId == id);
                response.Data = mapper.Map<ProductDto>(productbyId);
            }catch(Exception ex)
            {
               response.ErrorMessages = new List<string>{ex.Message};
               response.Success = false;
            }
            return response;
        }

        public async Task<ServiceResponse<ProductDto>> UpSertProduct(ProductDto productDto)
        {
            ServiceResponse<ProductDto> response = new ServiceResponse<ProductDto>();
             try
             {
                 var product = mapper.Map<ProductDto, Product>(productDto);
                 if(product.ProductId > 0)
                 {
                     dbContext.Products.Update(product);
                 }else
                 {
                     await dbContext.Products.AddAsync(product);
                 }
                 await dbContext.SaveChangesAsync();
                 response.Data= mapper.Map<Product, ProductDto>(product);
             }catch(Exception ex)
             {
                 response.Message = "Something Went Wrong";
                 response.ErrorMessages = new List<string>{ex.Message};
                 response.Success = false;
        
             }
             return response;
        }
    }
}