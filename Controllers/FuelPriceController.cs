using cs_project.Data;
using cs_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cs_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuelPriceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FuelPriceController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<FuelPrice>>> GetAllFuelPrices()
        {
            return await _context.FuelPrices.ToListAsync();

        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<FuelPrice>> GetFuelPrice(int id)
        {
            var fuelPrice = await _context.FuelPrices.FindAsync(id);
            if (fuelPrice == null) return NotFound();
            return Ok(id);
        }

        [HttpPost]
        public async Task<ActionResult> CreateFuelPrice([FromBody] FuelPrice fuelPrice)
        {
            _context.FuelPrices.Add(fuelPrice);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFuelPrice), new {id = fuelPrice.Id}, fuelPrice);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFuelPrice(int id, [FromBody] FuelPrice updatedFuelPrice)
        {
            var fuelPrice = await _context.FuelPrices.FindAsync(id);
            if (fuelPrice == null) return NotFound();

            fuelPrice.FuelType = updatedFuelPrice.FuelType;
            fuelPrice.CurrentPrice = updatedFuelPrice.CurrentPrice;
            fuelPrice.UpdatedAt = updatedFuelPrice.UpdatedAt;

            await _context.SaveChangesAsync();
            return Ok(fuelPrice);


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFuelPrice(int id)
        {
            var fuelPrice = await _context.FuelPrices.FindAsync(id);
            if (fuelPrice == null) return NotFound();
            
            _context.FuelPrices.Remove(fuelPrice);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
