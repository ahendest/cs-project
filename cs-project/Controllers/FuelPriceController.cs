using cs_project.Core.DTOs;
using cs_project.Core.Models;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cs_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FuelPriceController : ControllerBase
    {
        private readonly ILogger<FuelPriceController> _logger;
        private readonly IFuelPriceService _fuelPriceService;

        public FuelPriceController(IFuelPriceService fuelPriceService, ILogger<FuelPriceController> logger)
        {
            _logger = logger;
            _fuelPriceService = fuelPriceService;
        }

        [HttpGet]
        public async Task<ActionResult<List<FuelPriceDTO>>> GetAllFuelPrices()
        {
            var data = await _fuelPriceService.GetAllAsync();
            return Ok(data);

        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<FuelPriceDTO>> GetFuelPrice(int id)
        {
            var fuelPrice = await _fuelPriceService.GetByIdAsync(id);
            if (fuelPrice == null) return NotFound();
            return Ok(fuelPrice);
        }

        [HttpGet("query")]
        public async Task<ActionResult<PagedResult<FuelPriceDTO>>> QueryFuelPrices([FromQuery] PagingQueryParameters query)
        {
            var result = await _fuelPriceService.GetFuelPricesAsync(query);
            Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<FuelPriceDTO>> CreateFuelPrice([FromBody] FuelPriceCreateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdPrice = await _fuelPriceService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetFuelPrice), new {id = createdPrice.Id}, createdPrice);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFuelPrice(int id, [FromBody] FuelPriceCreateDTO dto)
        {
            _logger.LogInformation("FuelPrice updated!");
            var price = await _fuelPriceService.GetByIdAsync(id);
            if (price == null) return NotFound();
            
            await _fuelPriceService.UpdateAsync(id,dto);;
            return Ok(price);


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFuelPrice(int id)
        {
            var fuelPrice = await _fuelPriceService.GetByIdAsync(id);
            if (fuelPrice == null) return NotFound();

            await _fuelPriceService.DeleteAsync(id);
            return NoContent();
        }
    }
}
