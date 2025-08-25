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

        public CorrectionLogController(ICorrectionLogService correctionLogService)
        {
            _correctionLogService = correctionLogService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CorrectionLogDTO>>> GetAllCorrectionLogs()
        {
            var logs = await _correctionLogService.GetAllCorrectionLogsAsync();
            return Ok(logs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CorrectionLogDTO>> GetCorrectionLog(int id)
        {
            var log = await _correctionLogService.GetCorrectionLogByIdAsync(id);
            if (log == null) return NotFound();
            return Ok(log);
        }

        [HttpGet("query")]
        public async Task<ActionResult<PagedResult<CorrectionLogDTO>>> QueryCorrectionLogs([FromQuery] PagingQueryParameters query)
        {
            var result = await _correctionLogService.GetCorrectionLogAsync(query);
            Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());
            return Ok(result);
        }
    }
}

