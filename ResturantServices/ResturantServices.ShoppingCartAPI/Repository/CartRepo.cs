using ResturantServices.ShoppingCartAPI.Repository.IRepository;
using ResturantServices.ShoppingCartAPI.Data;
using ResturantServices.ShoppingCartAPI.Models.Dto;
using ResturantServices.ShoppingCartAPI.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ResturantServices.ShoppingCartAPI.Repository
{
    public class CartRepo : ICartRepo
    {
        private readonly ApplicationDbContext dbContext;
        private IMapper mapper;

        public CartRepo(ApplicationDbContext _dbcontext, IMapper _mapper)
        {
            dbContext = _dbcontext;
            mapper =_mapper;
        }
         public async Task<ServiceResponse<CartDto>> GetCartByUserId (string userId)
         {
            ServiceResponse<CartDto> response = new ServiceResponse<CartDto>();
            try
            {
               Cart cart = new()
               {
                   CartHeader = await dbContext.CartHeaders.FirstOrDefaultAsync(x =>
                                        x.UserId == userId)
               };
               cart.CartDetails = dbContext.CartDetails.Where(x=>
                                    x.CartHeaderId == cart.CartHeader.CartHeaderId)
                                    .Include(x=>x.Product);
                response.Data = mapper.Map<CartDto>(cart);
                return response;
            }catch(Exception ex)
            {
                response.ErrorMessages = new List<string>{ex.Message};
                response.Success=false;
            }
            return response;
        }

        public async Task<ServiceResponse<CartDto>> CreateUpdateCart (CartDto cartDto)
        {
            ServiceResponse<CartDto> response = new ServiceResponse<CartDto>();
            try
            {
                Cart cart = mapper.Map<Cart>(cartDto);
                var productinDb = await dbContext.Products.FirstOrDefaultAsync(x=>
                                        x.ProductId == cartDto.CartDetails.FirstOrDefault().ProductId);
                if(productinDb == null)
                {
                    dbContext.Products.Add(cart.CartDetails.FirstOrDefault().Product);
                    await dbContext.SaveChangesAsync();
                }
                //check if heaer is null
                var cartHeaderFromDb = await dbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(x=>
                                             x.UserId == cart.CartHeader.UserId);
                if(cartHeaderFromDb == null)
                {
                    //cretaes header and details 
                    dbContext.CartHeaders.Add(cart.CartHeader);
                    await dbContext.SaveChangesAsync();
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.CartHeaderId;
                    cart.CartDetails.FirstOrDefault().Product  =  null;
                    dbContext.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await dbContext.SaveChangesAsync();

                }
                else
                {
                    //checkif details has same product
                    var cartDetailsFromDb  = await dbContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(x =>
                                                  x.ProductId == cart.CartDetails.FirstOrDefault().ProductId &&
                                                  x.CartHeaderId == cartHeaderFromDb.CartHeaderId );
                    if(cartDetailsFromDb == null)
                    {
                        cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeaderFromDb .CartHeaderId;
                        cart.CartDetails.FirstOrDefault().Product  =  null;
                        dbContext.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                        await dbContext.SaveChangesAsync();
                    }else
                    {
                        //increase count
                        cart.CartDetails.FirstOrDefault().Product  =  null;

                        cart.CartDetails.FirstOrDefault().Count += cartDetailsFromDb.Count;
                        dbContext.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                        await dbContext.SaveChangesAsync();
                    }
                    response.Data = mapper.Map<CartDto>(cart);
                
                    return response;
                }

            }catch(Exception ex)
            {
                response.ErrorMessages = new List<string>{ex.Message};
                response.Success=false;
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> RemoveFromCart(int cartDetailsid)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();
            try
            {
               CartDetails cartDetails = await dbContext.CartDetails.FirstOrDefaultAsync(x=>
                                                x.CartDetailsId == cartDetailsid);
                int totalCountOfCartItem = dbContext.CartDetails.Where(x=>
                                            x.CartHeaderId == cartDetails.CartHeaderId).Count();
                dbContext.CartDetails.Remove(cartDetails);
                if(totalCountOfCartItem == 1)
                {
                    var cartHeaderToRemove = await dbContext.CartHeaders.FirstOrDefaultAsync(x =>
                                                    x.CartHeaderId == cartDetails.CartHeaderId);
                    dbContext.CartHeaders.Remove(cartHeaderToRemove);                    
                }   
                    await dbContext.SaveChangesAsync();
                    response.Data = true;
                    return response;

            }catch(Exception ex)
            {
                response.ErrorMessages = new List<string>{ex.Message};
                response.Success=false;
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> ClearCart(string userId)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();
            try
            {
               var cartHeaderFromDb = await dbContext.CartHeaders.FirstOrDefaultAsync(x =>
                                        x.UserId == userId);
               if(cartHeaderFromDb != null)
               {
                    dbContext.CartDetails.RemoveRange(dbContext.CartDetails.Where(x=>
                    x.CartHeaderId == cartHeaderFromDb.CartHeaderId));
                    dbContext.CartHeaders.Remove(cartHeaderFromDb);
                    dbContext.SaveChangesAsync();
                    response.Data = true;
                    return response;
               }
                response.Data=false;
                return response;
            }catch(Exception ex)
            {
                response.ErrorMessages = new List<string>{ex.Message};
                response.Success=false;
            }
            return response;
        }
    }
}