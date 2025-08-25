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
    public class SupplierPaymentApplyController : ControllerBase
    {
        private readonly ILogger<SupplierPaymentApplyController> _logger;
        private readonly ISupplierPaymentApplyService _applyService;

        public SupplierPaymentApplyController(ISupplierPaymentApplyService applyService, ILogger<SupplierPaymentApplyController> logger)
        {
            _logger = logger;
            _applyService = applyService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierPaymentApplyDTO>>> GetAllSupplierPaymentApplies()
        {
            var applies = await _applyService.GetAllSupplierPaymentAppliesAsync();
            return Ok(applies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierPaymentApplyDTO>> GetSupplierPaymentApply(int id)
        {
            var apply = await _applyService.GetSupplierPaymentApplyByIdAsync(id);
            if (apply == null) return NotFound();
            return Ok(apply);
        }

        [HttpGet("query")]
        public async Task<ActionResult<PagedResult<SupplierPaymentApplyDTO>>> QuerySupplierPaymentApplies([FromQuery] PagingQueryParameters query)
        {
            var result = await _applyService.GetSupplierPaymentApplyAsync(query);
            Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<SupplierPaymentApplyDTO>> CreateSupplierPaymentApply([FromBody] SupplierPaymentApplyCreateDTO createDto)
        {
            _logger.LogInformation("Supplier Payment Apply Created!");
            var dto = await _applyService.CreateSupplierPaymentApplyAsync(createDto);
            return CreatedAtAction(nameof(GetSupplierPaymentApply), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplierPaymentApply(int id, [FromBody] SupplierPaymentApplyCreateDTO updateDto)
        {
            var updated = await _applyService.UpdateSupplierPaymentApplyAsync(id, updateDto);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplierPaymentApply(int id)
        {
            var deleted = await _applyService.DeleteSupplierPaymentApplyAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
