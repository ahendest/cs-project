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
        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleDTO dto, CancellationToken ct)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var trx = await sales.CreateSaleAsync(dto.PumpId, dto.Liters, ct);
            return Ok(trx);
        }
    }
}
