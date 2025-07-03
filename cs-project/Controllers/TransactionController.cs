using cs_project.Core.DTOs;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionsDTO>>> GetAllTransactions()
    {
        var transactions = await _transactionService.GetAllAsync();

        return Ok(transactions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionsDTO>> GetTransaction(int id)
    {
        var transaction = await _transactionService.GetByIdAsync(id);

        return Ok(transaction);
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
