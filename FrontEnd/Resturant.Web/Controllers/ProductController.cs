using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Resturant.Web.Models;
using Resturant.Web.Services.IServices;
using System.Diagnostics;

namespace Resturant.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productservice;
        public ProductController(IProductService productService)
        {
            _productservice = productService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto> list = new();
            var acessToken = await HttpContext.GetTokenAsync("access_token");
            var res = await _productservice.GetAllProductsAsync<ProductDto>(acessToken);
            if (res != null && res.Success)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(res.Data));
            }
            return View(list);
        }
        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCreate(ProductDto product)
        {
            if (ModelState.IsValid)
            {
                var acessToken = await HttpContext.GetTokenAsync("access_token");

                var res = await _productservice.CreateProductAsync<ProductDto>(product, acessToken);
                if (res != null && res.Success)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }

            return View(product);
        }
        public async Task<IActionResult> ProductEdit(int productId)
        {
            var acessToken = await HttpContext.GetTokenAsync("access_token");

            var res = await _productservice.GetAllProductsIdAsync<ProductDto>(productId, acessToken);
            if (res != null && res.Success)
            {
                var productbyId = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(res.Data));
                return View(productbyId);

            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(ProductDto product)
        {
            if (ModelState.IsValid)
            {
                var acessToken = await HttpContext.GetTokenAsync("access_token");

                var res = await _productservice.UpdateProductAsync<ProductDto>(product, acessToken);
                if (res != null && res.Success)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }

            return View(product);
        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> ProductDelete(int productId)
        {
            var acessToken = await HttpContext.GetTokenAsync("access_token");

            var res = await _productservice.GetAllProductsIdAsync<ProductDto>(productId, acessToken);
            if (res != null && res.Success)
            {
                var productbyId = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(res.Data));
                return View(productbyId);

            }
            return NotFound();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDelete(ProductDto product)
        {
            if (ModelState.IsValid)
            {
                var acessToken = await HttpContext.GetTokenAsync("access_token");

                var res = await _productservice.DeleteProductAsync<ProductDto>(product.ProductId, acessToken);
                if (res.Success)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }

            return View(product);
        }
    }
}