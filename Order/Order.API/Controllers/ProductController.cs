using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        /*For Checking requist passing multiple port if one is used by other request.*/
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok();
        }
    }
}
