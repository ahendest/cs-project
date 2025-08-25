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
    public class TankController : ControllerBase
    {
        private readonly ILogger<TankController> _logger;
        private readonly ITankService _tankService;

        public TankController(ITankService tankService, ILogger<TankController> logger)
        {
            _logger = logger;
            _tankService = tankService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TankDTO>>> GetAllTanks()
        {
            var tanks = await _tankService.GetAllTanksAsync();
            return Ok(tanks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TankDTO>> GetTank(int id)
        {
            var tank = await _tankService.GetTankByIdAsync(id);
            if (tank == null) return NotFound();
            return Ok(tank);
        }

        [HttpGet("query")]
        public async Task<ActionResult<PagedResult<TankDTO>>> QueryTanks([FromQuery] PagingQueryParameters query)
        {
            var result = await _tankService.GetTankAsync(query);
            Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<TankDTO>> CreateTank([FromBody] TankCreateDTO createDto)
        {
            _logger.LogInformation("Tank Created!");
            var tankDto = await _tankService.CreateTankAsync(createDto);
            return CreatedAtAction(nameof(GetTank), new { id = tankDto.Id }, tankDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTank(int id, [FromBody] TankCreateDTO updateDto)
        {
            var updated = await _tankService.UpdateTankAsync(id, updateDto);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTank(int id)
        {
            var deleted = await _tankService.DeleteTankAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
