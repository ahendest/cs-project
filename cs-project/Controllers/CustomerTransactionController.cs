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
    public class CustomerTransactionController : ControllerBase
    {
        private readonly ILogger<CustomerTransactionController> _logger;
        private readonly ICustomerTransactionService _transactionService;

        public CustomerTransactionController(ICustomerTransactionService transactionService, ILogger<CustomerTransactionController> logger)
        {
            _logger = logger;
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerTransactionDTO>>> GetAllCustomerTransactions()
        {
            var transactions = await _transactionService.GetAllCustomerTransactionsAsync();
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerTransactionDTO>> GetCustomerTransaction(int id)
        {
            var transaction = await _transactionService.GetCustomerTransactionByIdAsync(id);
            if (transaction == null) return NotFound();
            return Ok(transaction);
        }

        [HttpGet("query")]
        public async Task<ActionResult<PagedResult<CustomerTransactionDTO>>> QueryCustomerTransactions([FromQuery] PagingQueryParameters query)
        {
            var result = await _transactionService.GetCustomerTransactionAsync(query);
            Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerTransactionDTO>> CreateCustomerTransaction([FromBody] CustomerTransactionCreateDTO createDto)
        {
            _logger.LogInformation("Customer Transaction Created!");
            var dto = await _transactionService.CreateCustomerTransactionAsync(createDto);
            return CreatedAtAction(nameof(GetCustomerTransaction), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomerTransaction(int id, [FromBody] CustomerTransactionCreateDTO updateDto)
        {
            var updated = await _transactionService.UpdateCustomerTransactionAsync(id, updateDto);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerTransaction(int id)
        {
            var deleted = await _transactionService.DeleteCustomerTransactionAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
