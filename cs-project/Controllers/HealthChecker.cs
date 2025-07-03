using cs_project.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class HealthController : ControllerBase
{
    private readonly AppDbContext _pumpService;
    public HealthController(AppDbContext context) => _pumpService = context;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var ok = await _pumpService.Database.CanConnectAsync();
        return Ok(new { DatabaseConnected = ok });
    }
}
