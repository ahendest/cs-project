using cs_project.Core.DTOs;
using cs_project.Core.Models;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cs_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ViewCorrectionLogs")]
    public class CorrectionLogController : ControllerBase
    {
        private readonly ICorrectionLogService _correctionLogService;
        private readonly ILogger<CorrectionLogController> _logger;

        public CorrectionLogController(ICorrectionLogService correctionLogService, ILogger<CorrectionLogController> logger)
        {
            _correctionLogService = correctionLogService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all correction logs.
        /// </summary>
        /// <returns>A list of correction logs.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CorrectionLogDTO>>> GetAllCorrectionLogs()
        {
            var logs = await _correctionLogService.GetAllCorrectionLogsAsync();
            return Ok(logs);
        }

        /// <summary>
        /// Gets a specific correction log by ID.
        /// </summary>
        /// <param name="id">The ID of the correction log.</param>
        /// <returns>The correction log with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CorrectionLogDTO>> GetCorrectionLog(int id)
        {
            var log = await _correctionLogService.GetCorrectionLogByIdAsync(id);
            if (log == null) return NotFound();
            return Ok(log);
        }

        /// <summary>
        /// Queries correction logs with pagination.
        /// </summary>
        /// <param name="query">The query parameters for pagination.</param>
        /// <returns>A paged result of correction logs.</returns>
        [HttpGet("query")]
        public async Task<ActionResult<PagedResult<CorrectionLogDTO>>> QueryCorrectionLogs([FromQuery] PagingQueryParameters query)
        {
            var result = await _correctionLogService.GetCorrectionLogAsync(query);
            Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());
            return Ok(result);
        }

        /// <summary>
        /// Creates a new correction log.
        /// </summary>
        /// <param name="dto">The data for the new correction log.</param>
        /// <returns>The created correction log.</returns>
        [HttpPost]
        public async Task<ActionResult<CorrectionLogDTO>> CreateCorrectionLog([FromBody] CorrectionLogCreateDTO dto)
        {
            var created = await _correctionLogService.CreateCorrectionLogAsync(dto);
            _logger.LogInformation("CorrectionLog created: {@created}", created);
            return CreatedAtAction(nameof(GetCorrectionLog), new { id = created.Id }, created);
        }

        /// <summary>
        /// Updates an existing correction log.
        /// </summary>
        /// <param name="id">The ID of the correction log to update.</param>
        /// <param name="dto">The new data for the correction log.</param>
        /// <returns>No content if successful, not found otherwise.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCorrectionLog(int id, [FromBody] CorrectionLogCreateDTO dto)
        {
            var ok = await _correctionLogService.UpdateCorrectionLogAsync(id, dto);
            return ok ? NoContent() : NotFound();
        }

        /// <summary>
        /// Deletes a correction log.
        /// </summary>
        /// <param name="id">The ID of the correction log to delete.</param>
        /// <returns>No content if successful, not found otherwise.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCorrectionLog(int id)
        {
            var ok = await _correctionLogService.DeleteCorrectionLogAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}

