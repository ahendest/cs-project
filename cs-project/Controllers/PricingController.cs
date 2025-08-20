using cs_project.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace cs_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PricingController(IPricingService pricing) : ControllerBase
    {
        [HttpPost("reprice/{stationId:int}")]
        public async Task<IActionResult> Reprice(int stationId, CancellationToken ct)
        {
            var result = await pricing.RepriceStationAsync(stationId, null, ct);
            return Ok(result);
        }

        [HttpGet("{stationId:int}/current")]
        public async Task<IActionResult> Current(int stationId, CancellationToken ct)
        {
            var result = await pricing.GetCurrentPriceAsync(stationId, ct);
            return Ok(result);
        }

    }
}
