using Microsoft.AspNetCore.Mvc;
using Resturant.Web.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Resturant.Web.Services.IServices;
using Newtonsoft.Json;

namespace Resturant.Web.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService productService;

    public HomeController(ILogger<HomeController> logger,IProductService _productService)
    {
        _logger = logger;
        productService = _productService; 
    }
     
    public async Task<IActionResult> Index()
    {
        List<ProductDto> productList = new ();
        var res = await productService.GetAllProductsAsync<ProductDto>("");
        if(res !=null && res.Success)
        {
            productList =  JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(res.Data));

        }
        return View(productList);
    }
    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        ProductDto productList = new ();
        // var token = await HttpContext.GetTokenAsync("access_token");
        var res = await productService.GetAllProductsIdAsync<ProductDto>(id,"");
        if(res !=null && res.Success)
        {
            productList =  JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(res.Data));

        }
        return View(productList);
    }
    [Authorize]
    public  IActionResult Login()
    {
        // var Token = await HttpContext.GetTokenAsync("access_token");
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Logout()
    {
        return SignOut("Cookies","oidc");
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
