using Microsoft.AspNetCore.Mvc;
using ResturantServices.ShoppingCartAPI.Repository.IRepository;
using ResturantServices.ShoppingCartAPI.Models;
using ResturantServices.ShoppingCartAPI.Models.Dto;
using ResturantServices.ShoppingCartAPI.Repository;
using Microsoft.AspNetCore.Authorization;

namespace ResturantServices.ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepo icarRepo;
        public CartController(ICartRepo _icartRepo)
        {
            icartRepo = _icartRepo;
        }

        [HttpGet("GetCart/{id}")]
        public async Task<IActionResult> GetCart(string userId)
        {
            
            var response = new ServiceResponse<CartDto>();
            try
            {
                response = await icartRepo.GetCartByUserId(userId);
                if (response.Success)
                {
                    return Ok(response);
                }
                return NotFound(response);

            }
            catch (System.Exception ex)
            {
                response.ErrorMessages = new List<string> { ex.Message };
                response.Success = false;
                return BadRequest(response);
            }
        }
        
        [HttpPost("AddCart")]
        public async Task<IActionResult> AddCart(CartDto cartDto)
        {
            
            var response = new ServiceResponse<CartDto>();
            try
            {
                response = await icartRepo.CreateUpdateCart(cartDto);
                if (response.Success)
                {
                    return Ok(response);
                }
                return NotFound(response);

            }
            catch (System.Exception ex)
            {
                response.ErrorMessages = new List<string> { ex.Message };
                response.Success = false;
                return BadRequest(response);
            }
        }
        [HttpPost("UpdateCart")]
        public async Task<IActionResult> UpdateCart(CartDto cartDto)
        {
            
            var response = new ServiceResponse<CartDto>();
            try
            {
                response = await icartRepo.CreateUpdateCart(cartDto);
                if (response.Success)
                {
                    return Ok(response);
                }
                return NotFound(response);

            }
            catch (System.Exception ex)
            {
                response.ErrorMessages = new List<string> { ex.Message };
                response.Success = false;
                return BadRequest(response);
            }
        }
        
        [HttpPost("RemoveCart")]
        public async Task<IActionResult> RemoveCart([FromBody]int carId)
        {
            
            var response = new ServiceResponse<bool>();
            try
            {
                response = await icartRepo.RemoveFromCart(cartId);
                if (response.Success)
                {
                    return Ok(response);
                }
                return NotFound(response);

            }
            catch (System.Exception ex)
            {
                response.ErrorMessages = new List<string> { ex.Message };
                response.Success = false;
                return BadRequest(response);
            }
        }

        [HttpPost("ClearCart")]
        public async Task<IActionResult> ClearCart(string userId)
        {
            
            var response = new ServiceResponse<bool>();
            try
            {
                response = await icartRepo.ClearCart(userId);
                if (response.Success)
                {
                    return Ok(response);
                }
                return NotFound(response);

            }
            catch (System.Exception ex)
            {
                response.ErrorMessages = new List<string> { ex.Message };
                response.Success = false;
                return BadRequest(response);
            }
        }
    }
}