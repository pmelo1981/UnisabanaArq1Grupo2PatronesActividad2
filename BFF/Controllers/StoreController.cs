using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace BFF.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _backendUrl;
        public StoreController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _backendUrl = config["BackendUrl"];
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProducts()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_backendUrl}/api/products");
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Error al obtener productos del backend");

            var json = await response.Content.ReadAsStringAsync();
            var products = JsonSerializer.Deserialize<dynamic>(json);

            // Detectar tipo de cliente por header
            var clientType = Request.Headers["X-Client-Type"].ToString().ToLower();
            var adapted = new System.Collections.Generic.List<object>();
            if (clientType == "mobile")
            {
                // Adaptar para móvil: solo name y stock
                foreach (var p in products.EnumerateArray())
                {
                    adapted.Add(new {
                        Name = p.GetProperty("name").GetString(),
                        Stock = p.GetProperty("stock").GetInt32()
                    });
                }
            }
            else // web o default
            {
                // Adaptar para web: solo nombre y precio
                foreach (var p in products.EnumerateArray())
                {
                    adapted.Add(new {
                        Nombre = p.GetProperty("name").GetString(),
                        Precio = p.GetProperty("price").GetInt32()
                    });
                }
            }
            return Ok(adapted);
        }
    }
}