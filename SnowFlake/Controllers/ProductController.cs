using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SnowFlake.Dtos.APIs.Product.GetProducts;
using SnowFlake.Services;

namespace SnowFlake.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(string ownerId)
    {
        try
        {

            var products = await _productService.GetProductsByOwnerId(ownerId);
            return Ok( );
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}