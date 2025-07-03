using cs_project.Core.DTOs;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cs_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PumpsController : ControllerBase
    {
        private readonly IPumpService _pumpService;

        public PumpsController(IPumpService pumpService)
        {
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

        [HttpPost]
        public async Task<ActionResult<PumpDTO>> CreatePump([FromBody] PumpCreateDTO createDto)
        {
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
            await _pumpService.DeletePumpAsync(id);
            return NoContent();
        }
    }
}
