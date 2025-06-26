using cs_project.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly AppDbContext _context;
    public HealthController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var ok = await _context.Database.CanConnectAsync();
        return Ok(new { DatabaseConnected = ok });
    }
}
