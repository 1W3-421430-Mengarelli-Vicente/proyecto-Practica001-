using _1TransaccionesProductos.Domain;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturasController : ControllerBase
    {
        private readonly IFacturaService? _facturaService;

        public FacturasController()
        {
            _facturaService = new FacturaService();
        }

        [HttpGet]
        public IActionResult GetFacturas()
        {
            var facturas = _facturaService.GetAllInvoices();
            return Ok(facturas);
        }

        [HttpPost]
        public IActionResult PostFacturas([FromBody] Invoice invoice)
        {
            var result = _facturaService.CreateInvoice(invoice);
            if (result)
                return Ok("Factura registrada correctamente");
            else return BadRequest("Fallo al intentar registrar la factura");
        }

        [HttpDelete("{numero}")]
        public IActionResult DeleteFacturas(int numero)
        {
            var result = _facturaService.DeleteInvoice(numero);
            if (result)
                return Ok("Factura eliminada correctamente");
            else
                return NotFound("Fallo al intentar eliminar la factura");
        }
        
    }
}
