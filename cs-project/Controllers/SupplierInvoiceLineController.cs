using cs_project.Core.DTOs;
using cs_project.Core.Models;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cs_project.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SupplierInvoiceLineController : ControllerBase
{
    private readonly ILogger<SupplierInvoiceLineController> _logger;
    private readonly ISupplierInvoiceLineService _lineService;

    public SupplierInvoiceLineController(ISupplierInvoiceLineService lineService, ILogger<SupplierInvoiceLineController> logger)
    {
        _logger = logger;
        _lineService = lineService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SupplierInvoiceLineDTO>>> GetAllLines()
    {
        var lines = await _lineService.GetAllSupplierInvoiceLinesAsync();
        return Ok(lines);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SupplierInvoiceLineDTO>> GetLine(int id)
    {
        var line = await _lineService.GetSupplierInvoiceLineByIdAsync(id);
        if (line == null) return NotFound();
        return Ok(line);
    }

    [HttpGet("query")]
    public async Task<ActionResult<PagedResult<SupplierInvoiceLineDTO>>> QueryLines([FromQuery] PagingQueryParameters query)
    {
        var result = await _lineService.GetSupplierInvoiceLineAsync(query);
        Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<SupplierInvoiceLineDTO>> CreateLine([FromBody] SupplierInvoiceLineCreateDTO dto)
    {
        var created = await _lineService.CreateSupplierInvoiceLineAsync(dto);
        return CreatedAtAction(nameof(GetLine), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLine(int id, [FromBody] SupplierInvoiceLineCreateDTO dto)
    {
        var updated = await _lineService.UpdateSupplierInvoiceLineAsync(id, dto);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLine(int id)
    {
        var deleted = await _lineService.DeleteSupplierInvoiceLineAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
