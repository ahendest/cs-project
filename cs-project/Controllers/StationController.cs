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
    public class StationController : ControllerBase
    {
        private readonly ILogger<StationController> _logger;
        private readonly IStationService _stationService;

        public StationController(IStationService stationService, ILogger<StationController> logger)
        {
            _logger = logger;
            _stationService = stationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StationDTO>>> GetAllStations(CancellationToken ct)
        {
            var stations = await _stationService.GetAllStationsAsync(ct);
            return Ok(stations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StationDTO>> GetStation(int id, CancellationToken ct)
        {
            var station = await _stationService.GetStationByIdAsync(id, ct);
            if (station == null) return NotFound();
            return Ok(station);
        }

        [HttpGet("query")]
        public async Task<ActionResult<PagedResult<StationDTO>>> QueryStations([FromQuery] PagingQueryParameters query, CancellationToken ct)
        {
            var result = await _stationService.GetStationAsync(query, ct);
            Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<StationDTO>> CreateStation([FromBody] StationCreateDTO createDto, CancellationToken ct)
        {
            _logger.LogInformation("Station Created!");
            var stationDto = await _stationService.CreateStationAsync(createDto, ct);
            return CreatedAtAction(nameof(GetStation), new { id = stationDto.Id }, stationDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStation(int id, [FromBody] StationCreateDTO updateDto, CancellationToken ct)
        {
            var updated = await _stationService.UpdateStationAsync(id, updateDto, ct);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStation(int id, CancellationToken ct)
        {
            var deleted = await _stationService.DeleteStationAsync(id, ct);
            return deleted ? NoContent() : NotFound();
        }
    }
}
