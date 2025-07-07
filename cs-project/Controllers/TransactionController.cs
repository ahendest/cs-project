using cs_project.Core.DTOs;
using cs_project.Core.Models;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly ILogger<TransactionController> _logger;
    public TransactionController(ITransactionService transactionService,ILogger<TransactionController> logger)
    {
        _logger = logger;
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionsDTO>>> GetAllTransactions()
    {
        _logger.LogInformation("GetAllTransactions called");
        var transactions = await _transactionService.GetAllAsync();
        return Ok(transactions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionsDTO>> GetTransaction(int id)
    {
        var transaction = await _transactionService.GetByIdAsync(id);
        if (transaction == null)
            return NotFound();

        return Ok(transaction);
    }

    [HttpGet("query")]
    public async Task<ActionResult<PagedResult<TransactionsDTO>>> QueryTransactions([FromQuery] PagingQueryParameters query)
    {
        var result = await _transactionService.GetTransactionsAsync(query);
        Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<TransactionsDTO>> CreateTransaction([FromBody] TransactionsCreateDTO dto)
    {
        var createdTransactionDto = await _transactionService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetTransaction), new { id = createdTransactionDto.Id }, createdTransactionDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTransaction(int id, [FromBody] TransactionsCreateDTO dto)
    {
        var updatedTransaction = await _transactionService.UpdateAsync(id, dto);

        return updatedTransaction ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(int id)
    {
        var deletedTransaction = await _transactionService.DeleteAsync(id);
        return deletedTransaction ? NoContent() : NotFound();
    }
}
