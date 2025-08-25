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
    public class SupplierPaymentController : ControllerBase
    {
        private readonly ILogger<SupplierPaymentController> _logger;
        private readonly ISupplierPaymentService _paymentService;

        public SupplierPaymentController(ISupplierPaymentService paymentService, ILogger<SupplierPaymentController> logger)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierPaymentDTO>>> GetAllSupplierPayments()
        {
            var payments = await _paymentService.GetAllSupplierPaymentsAsync();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierPaymentDTO>> GetSupplierPayment(int id)
        {
            var payment = await _paymentService.GetSupplierPaymentByIdAsync(id);
            if (payment == null) return NotFound();
            return Ok(payment);
        }

        [HttpGet("query")]
        public async Task<ActionResult<PagedResult<SupplierPaymentDTO>>> QuerySupplierPayments([FromQuery] PagingQueryParameters query)
        {
            var result = await _paymentService.GetSupplierPaymentAsync(query);
            Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<SupplierPaymentDTO>> CreateSupplierPayment([FromBody] SupplierPaymentCreateDTO createDto)
        {
            _logger.LogInformation("Supplier Payment Created!");
            var dto = await _paymentService.CreateSupplierPaymentAsync(createDto);
            return CreatedAtAction(nameof(GetSupplierPayment), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplierPayment(int id, [FromBody] SupplierPaymentCreateDTO updateDto)
        {
            var updated = await _paymentService.UpdateSupplierPaymentAsync(id, updateDto);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplierPayment(int id)
        {
            var deleted = await _paymentService.DeleteSupplierPaymentAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
