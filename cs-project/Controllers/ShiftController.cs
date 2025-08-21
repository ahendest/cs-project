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
    public class ShiftController : ControllerBase
    {
        private readonly ILogger<ShiftController> _logger;
        private readonly IShiftService _shiftService;

        public ShiftController(IShiftService shiftService, ILogger<ShiftController> logger)
        {
            _logger = logger;
            _shiftService = shiftService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShiftDTO>>> GetAllShifts()
        {
            var shifts = await _shiftService.GetAllShiftsAsync();
            return Ok(shifts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShiftDTO>> GetShift(int id)
        {
            var shift = await _shiftService.GetShiftByIdAsync(id);
            if (shift == null) return NotFound();
            return Ok(shift);
        }

        [HttpGet("query")]
        public async Task<ActionResult<PagedResult<ShiftDTO>>> QueryShifts([FromQuery] PagingQueryParameters query)
        {
            var result = await _shiftService.GetShiftAsync(query);
            Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ShiftDTO>> CreateShift([FromBody] ShiftCreateDTO createDto)
        {
            _logger.LogInformation("Shift Created!");
            var shiftDto = await _shiftService.CreateShiftAsync(createDto);
            return CreatedAtAction(nameof(GetShift), new { id = shiftDto.Id }, shiftDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShift(int id, [FromBody] ShiftCreateDTO updateDto)
        {
            var updated = await _shiftService.UpdateShiftAsync(id, updateDto);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShift(int id)
        {
            var deleted = await _shiftService.DeleteShiftAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}

