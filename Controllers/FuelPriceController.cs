using cs_project.Infrastructure.Data;
using cs_project.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using cs_project.Core.DTOs;

namespace cs_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuelPriceController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FuelPriceController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<FuelPriceDTO>>> GetAllFuelPrices()
        {
            var data = await _context.FuelPrices.ToListAsync();
            return Ok(_mapper.Map<List<FuelPriceDTO>>(data));

        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<FuelPriceDTO>> GetFuelPrice(int id)
        {
            var fuelPrice = await _context.FuelPrices.FindAsync(id);
            if (fuelPrice == null) return NotFound();
            return Ok(_mapper.Map<FuelPriceDTO>(fuelPrice));
        }

        [HttpPost]
        public async Task<ActionResult> CreateFuelPrice([FromBody] FuelPriceCreateDTO dto)
        {
            var price = _mapper.Map<FuelPrice>(dto);
            _context.FuelPrices.Add(price);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFuelPrice), new {id = price.Id}, _mapper.Map<FuelPriceDTO>(price));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFuelPrice(int id, [FromBody] FuelPriceCreateDTO dto)
        {
            var price = await _context.FuelPrices.FindAsync(id);
            if (price == null) return NotFound();
            
            _mapper.Map(dto, price);

            await _context.SaveChangesAsync();
            return Ok(price);


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
