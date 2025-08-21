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
    public class SupplierController : ControllerBase
    {
        private readonly ILogger<SupplierController> _logger;
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService, ILogger<SupplierController> logger)
        {
            _logger = logger;
            _supplierService = supplierService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierDTO>>> GetAllSuppliers()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            return Ok(suppliers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierDTO>> GetSupplier(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null) return NotFound();
            return Ok(supplier);
        }

        [HttpGet("query")]
        public async Task<ActionResult<PagedResult<SupplierDTO>>> QuerySuppliers([FromQuery] PagingQueryParameters query)
        {
            var result = await _supplierService.GetSupplierAsync(query);
            Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<SupplierDTO>> CreateSupplier([FromBody] SupplierCreateDTO createDto)
        {
            _logger.LogInformation("Supplier Created!");
            var supplierDto = await _supplierService.CreateSupplierAsync(createDto);
            return CreatedAtAction(nameof(GetSupplier), new { id = supplierDto.Id }, supplierDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] SupplierCreateDTO updateDto)
        {
            var updated = await _supplierService.UpdateSupplierAsync(id, updateDto);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var deleted = await _supplierService.DeleteSupplierAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}

