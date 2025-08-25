using cs_project.Core.DTOs;
using cs_project.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cs_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SalesController(ISalesService sales) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetSales(CancellationToken ct)
        {
            var list = await sales.GetSalesAsync(ct);
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSale(int id, CancellationToken ct)
        {
            var trx = await sales.GetSaleByIdAsync(id, ct);
            return trx is null ? NotFound() : Ok(trx);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleDTO dto, CancellationToken ct)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var trx = await sales.CreateSaleAsync(dto.PumpId, dto.Liters, ct);
            return Ok(trx);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSale(int id, [FromBody] CreateSaleDTO dto, CancellationToken ct)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var updated = await sales.UpdateSaleAsync(id, dto.PumpId, dto.Liters, ct);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(int id, CancellationToken ct)
        {
            var deleted = await sales.DeleteSaleAsync(id, ct);
            return deleted ? NoContent() : NotFound();
        }
    }
}
