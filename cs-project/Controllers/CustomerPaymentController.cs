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
        public async Task<ActionResult<IEnumerable<CustomerPaymentDTO>>> GetAllCustomerPayments()
        {
            var payments = await _paymentService.GetAllCustomerPaymentsAsync();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerPaymentDTO>> GetCustomerPayment(int id)
        {
            var payment = await _paymentService.GetCustomerPaymentByIdAsync(id);
            if (payment == null) return NotFound();
            return Ok(payment);
        }

        [HttpGet("query")]
        public async Task<ActionResult<PagedResult<CustomerPaymentDTO>>> QueryCustomerPayments([FromQuery] PagingQueryParameters query)
        {
            var result = await _paymentService.GetCustomerPaymentAsync(query);
            Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerPaymentDTO>> CreateCustomerPayment([FromBody] CustomerPaymentCreateDTO createDto)
        {
            _logger.LogInformation("Customer Payment Created!");
            var dto = await _paymentService.CreateCustomerPaymentAsync(createDto);
            return CreatedAtAction(nameof(GetCustomerPayment), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomerPayment(int id, [FromBody] CustomerPaymentCreateDTO updateDto)
        {
            var updated = await _paymentService.UpdateCustomerPaymentAsync(id, updateDto);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerPayment(int id)
        {
            var deleted = await _paymentService.DeleteCustomerPaymentAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
