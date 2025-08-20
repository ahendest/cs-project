using cs_project.Core.DTOs;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using cs_project.Core.Models;

namespace cs_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PumpController : ControllerBase
    {
        private readonly ILogger<PumpController> _logger;
        private readonly IPumpService _pumpService;

        public PumpController(IPumpService pumpService, ILogger<PumpController> logger)
        {
            _logger = logger;
            _pumpService = pumpService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PumpDTO>>> GetAllPumps()
        {
            var pumps = await _pumpService.GetAllPumpsAsync();
            return Ok(pumps);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PumpDTO>> GetPump(int id)
        {
            var pump = await _pumpService.GetPumpByIdAsync(id);
            if (pump == null) return NotFound();
            return Ok(pump);
        }

        [HttpGet("query")]
        public async Task<ActionResult<PagedResult<PumpDTO>>> QueryPumps([FromQuery] PagingQueryParameters query)
        {
            var result = await _pumpService.GetPumpAsync(query);
            Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<PumpDTO>> CreatePump([FromBody] PumpCreateDTO createDto)
        {
            _logger.LogInformation("Pump Created!");
            var pumpDto = await _pumpService.CreatePumpAsync(createDto);

            return CreatedAtAction(nameof(GetPump), new { id = pumpDto.Id }, pumpDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePump(int id, [FromBody] PumpCreateDTO updateDto)
        {
            var pump = await _pumpService.UpdatePumpAsync(id, updateDto);

            return pump ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePump(int id)
        {
            var deleted = await _pumpService.DeletePumpAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
