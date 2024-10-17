using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Service.IService;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Domain.Entities.Product product)
        {
            await _productService.AddProduct(product);
            return Ok("Inserted successfully");
        }

        [HttpPut]
        public async Task<IActionResult> EditProduct(Domain.Entities.Product product)
        {
            await _productService.EditProduct(product);
            return Ok("Updated successfully");
        }

        [HttpGet("GetByProductid")]
        public async Task<IActionResult> GetByProductid(int id)
        {
            return Ok(await _productService.GetById(id));
        }

        [HttpGet("GetProductQuantity")]
        public async Task<IActionResult> GetProductQuantity(int id)
        {
            var product = await _productService.GetById(id);
            return Ok(product.Quantity);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _productService.GetAllProducts());
        }
    }
}
