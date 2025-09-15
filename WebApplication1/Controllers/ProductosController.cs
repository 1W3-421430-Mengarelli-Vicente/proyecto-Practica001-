using _1TransaccionesProductos.Domain;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductosController(IProductService productService)
        {
            _service = productService;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            List<_1TransaccionesProductos.Domain.Product> products = _service.GetAll();
            if (products == null || products.Count == 0)
            {
                return NotFound("No products found.");
            }
            return Ok(products);
        }
        [HttpGet("{code}")]
        public IActionResult GetById(string code)
        {
            var product = _service.GetById(code);
            if (product == null)
            {
                return NotFound($"Product with code {code} not found.");
            }
            return Ok(product);
        }
        [HttpPost]
        public IActionResult Create([FromBody] _1TransaccionesProductos.Domain.Product product)
        {
            if (product == null)
            {
                return BadRequest("Product is null.");
            }
            bool created = _service.Create(product);
            if (!created)
            {
                return Conflict("Product with the same code already exists.");
            }
            return CreatedAtAction(nameof(GetById), new { code = product.Code }, product);
        }
        [HttpPut("{code}")]
        public IActionResult Update(string code, [FromBody] Product product)
        {
            if (product == null || product.Code != code)
            {
                return BadRequest("Product is null or code mismatch.");
            }
            bool updated = _service.Update(product);
            if (!updated)
            {
                return NotFound($"Product with code {code} not found.");
            }
            return NoContent();
        }
        [HttpDelete("{code}")]
        public IActionResult Delete(string code)
        {
            bool deleted = _service.Delete(code);
            if (!deleted)
            {
                return NotFound($"Product with code {code} not found.");
            }
            return NoContent();
        }


    }
}
