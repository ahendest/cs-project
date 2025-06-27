using cs_project.Infrastructure.Data;
using cs_project.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using cs_project.Core.DTOs;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public TransactionController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionsDTO>>> GetAllTransactions()
    {
        var data = await _context.Transactions.ToListAsync();
        return Ok(_mapper.Map<List<TransactionsDTO>>(data));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionsDTO>> GetTransaction(int id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null) return NotFound();
        return Ok(_mapper.Map<TransactionsDTO>(transaction));
    }

    [HttpPost]
    public async Task<ActionResult<TransactionsDTO>> CreateTransaction([FromBody] TransactionsCreateDTO dto)
    {
        var transaction = _mapper.Map<Transaction>(dto);
        transaction.TotalPrice = dto.Liters * dto.PricePerLiter;
        transaction.Timestamp = DateTime.UtcNow;

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, _mapper.Map<TransactionsDTO>(transaction));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTransaction(int id, [FromBody] TransactionsCreateDTO dto)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null) return NotFound();

        var price = await _context.FuelPrices.FindAsync(id);
        if (price == null) return NotFound();

        _mapper.Map(dto, price);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(int id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null) return NotFound();

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();
        return NoContent();

    }
}
