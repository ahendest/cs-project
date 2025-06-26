using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cs_project.Data;
using cs_project.Models;

namespace cs_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PumpsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PumpsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pump>>> GetAllPumps()
        {
            var pumps = await _context.Pumps.ToListAsync();
            return Ok(pumps);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pump>> GetPump(int id)
        {
            var pump = await _context.Pumps.FindAsync(id);
            if (pump == null) return NotFound();
            return Ok(pump);
        }

        [HttpPost]
        public async Task<ActionResult<Pump>> CreatePump([FromBody] Pump pump)
        {
            _context.Pumps.Add(pump);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPump), new { id = pump.Id }, pump);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePump(int id, [FromBody] Pump updatedPump)
        {
            var pump = await _context.Pumps.FindAsync(id);
            if (pump == null) return NotFound();

            pump.Status = updatedPump.Status;
            pump.CurrentVolume = updatedPump.CurrentVolume;
            pump.FuelType = updatedPump.FuelType;

            await _context.SaveChangesAsync();
            return Ok(pump);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePump(int id)
        {
            var pump = await _context.Pumps.FindAsync(id);
            if (pump == null)
                return NotFound();

            _context.Pumps.Remove(pump);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
