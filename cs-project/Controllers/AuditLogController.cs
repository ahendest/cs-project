using cs_project.Core.DTOs;
using cs_project.Core.Models;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cs_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ViewAuditLogs")]
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLogService _auditLogService;

        public AuditLogController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuditLogDTO>>> GetAllAuditLogs()
        {
            var logs = await _auditLogService.GetAllAuditLogsAsync();
            return Ok(logs);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<AuditLogDTO>> GetAuditLog(long id)
        {
            var log = await _auditLogService.GetAuditLogByIdAsync(id);
            if (log == null) return NotFound();
            return Ok(log);
        }

        [HttpGet("query")]
        public async Task<ActionResult<PagedResult<AuditLogDTO>>> QueryAuditLogs([FromQuery] PagingQueryParameters query)
        {
            var result = await _auditLogService.GetAuditLogAsync(query);
            Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());
            return Ok(result);
        }
    }
}

