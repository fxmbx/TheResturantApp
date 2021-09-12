using Microsoft.AspNetCore.Mvc;
using ResturantServices.ProductApi.Models;
using ResturantServices.ProductApi.Models.Dto;
using ResturantServices.ProductApi.Repository;
using Microsoft.AspNetCore.Authorization;

namespace ResturantServices.ProductApi.Controllers
{
    [Route("api/[controller]")]
    public class ProductsApiController : ControllerBase
    {
        private IProductRepo iproduct;

        public ProductsApiController(IProductRepo _iproduct)
        {
            iproduct = _iproduct;

        }
        // [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetProduct()
        {
            var response = await iproduct.GetProducts();
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        // [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductbyId(int id)
        {
            var response = await iproduct.GetProductsById(id);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);

        }
        [Authorize]
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto req)
        {
            var response = await iproduct.UpSertProduct(req);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize]
        [HttpPut("EditProduct")]
        public async Task<IActionResult> EditProducct([FromBody] ProductDto req)
        {
            var response = await iproduct.UpSertProduct(req);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                response = await iproduct.DeleteProdut(id);
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