using cs_project.Core.DTOs;
using cs_project.Core.Models;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cs_project.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SupplierInvoiceController : ControllerBase
{
    private readonly ILogger<SupplierInvoiceController> _logger;
    private readonly ISupplierInvoiceService _invoiceService;
    private readonly ISupplierInvoiceLineService _lineService;

    public SupplierInvoiceController(ISupplierInvoiceService invoiceService,
        ISupplierInvoiceLineService lineService,
        ILogger<SupplierInvoiceController> logger)
    {
        _logger = logger;
        _invoiceService = invoiceService;
        _lineService = lineService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SupplierInvoiceDTO>>> GetAllInvoices()
    {
        var invoices = await _invoiceService.GetAllSupplierInvoicesAsync();
        return Ok(invoices);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SupplierInvoiceDTO>> GetInvoice(int id)
    {
        var invoice = await _invoiceService.GetSupplierInvoiceByIdAsync(id);
        if (invoice == null) return NotFound();
        return Ok(invoice);
    }

    [HttpGet("{id}/lines")]
    public async Task<ActionResult<IEnumerable<SupplierInvoiceLineDTO>>> GetInvoiceLines(int id)
    {
        var invoice = await _invoiceService.GetSupplierInvoiceByIdAsync(id);
        if (invoice == null) return NotFound();
        var lines = await _lineService.GetLinesByInvoiceIdAsync(id);
        return Ok(lines);
    }

    [HttpGet("query")]
    public async Task<ActionResult<PagedResult<SupplierInvoiceDTO>>> QueryInvoices([FromQuery] PagingQueryParameters query)
    {
        var result = await _invoiceService.GetSupplierInvoiceAsync(query);
        Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<SupplierInvoiceDTO>> CreateInvoice([FromBody] SupplierInvoiceCreateDTO dto)
    {
        var created = await _invoiceService.CreateSupplierInvoiceAsync(dto);
        return CreatedAtAction(nameof(GetInvoice), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInvoice(int id, [FromBody] SupplierInvoiceCreateDTO dto)
    {
        var updated = await _invoiceService.UpdateSupplierInvoiceAsync(id, dto);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInvoice(int id)
    {
        var deleted = await _invoiceService.DeleteSupplierInvoiceAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
