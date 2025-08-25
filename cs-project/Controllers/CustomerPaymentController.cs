using cs_project.Core.DTOs;
using cs_project.Core.Models;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace cs_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerPaymentController : ControllerBase
    {
        private readonly ILogger<CustomerPaymentController> _logger;
        private readonly ICustomerPaymentService _paymentService;

        public CustomerPaymentController(ICustomerPaymentService paymentService, ILogger<CustomerPaymentController> logger)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerPaymentDTO>>> GetAllCustomerPayments(CancellationToken ct)
        {
            var payments = await _paymentService.GetAllCustomerPaymentsAsync(ct);
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerPaymentDTO>> GetCustomerPayment(int id, CancellationToken ct)
        {
            var payment = await _paymentService.GetCustomerPaymentByIdAsync(id, ct);
            if (payment == null) return NotFound();
            return Ok(payment);
        }

        [HttpGet("query")]
        public async Task<ActionResult<PagedResult<CustomerPaymentDTO>>> QueryCustomerPayments([FromQuery] PagingQueryParameters query, CancellationToken ct)
        {
            var result = await _paymentService.GetCustomerPaymentAsync(query, ct);
            Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerPaymentDTO>> CreateCustomerPayment([FromBody] CustomerPaymentCreateDTO createDto, CancellationToken ct)
        {
            _logger.LogInformation("Customer Payment Created!");
            var dto = await _paymentService.CreateCustomerPaymentAsync(createDto, ct);
            return CreatedAtAction(nameof(GetCustomerPayment), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomerPayment(int id, [FromBody] CustomerPaymentCreateDTO updateDto, CancellationToken ct)
        {
            var updated = await _paymentService.UpdateCustomerPaymentAsync(id, updateDto, ct);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerPayment(int id, CancellationToken ct)
        {
            var deleted = await _paymentService.DeleteCustomerPaymentAsync(id, ct);
            return deleted ? NoContent() : NotFound();
        }
    }
}
