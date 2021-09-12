using ResturantServices.ShoppingCartAPI.Models;
using ResturantServices.ShoppingCartAPI.Models.Dto;
namespace ResturantServices.ShoppingCartAPI.Repository.IRepository
{
    public interface ICartRepo
    {
        Task<ServiceResponse<CartDto>> GetCartByUserId (string userId);
        Task<ServiceResponse<CartDto>> CreateUpdateCart (CartDto cartDto);
        Task<ServiceResponse<bool>> RemoveFromCart(int cartDetailsid);
        Task<ServiceResponse<bool>> ClearCart(string userId);


    }
}