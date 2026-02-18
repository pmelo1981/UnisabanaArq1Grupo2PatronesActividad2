using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = new[]
            {
                new { Id = 1, Name = "Laptop", Price = 1200, Stock = 10 },
                new { Id = 2, Name = "Mouse", Price = 25, Stock = 100 },
                new { Id = 3, Name = "Keyboard", Price = 45, Stock = 50 }
            };
            return Ok(products);
        }
    }
}