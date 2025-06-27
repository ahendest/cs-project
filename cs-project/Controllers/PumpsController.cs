using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Infrastructure.Data;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PumpsController : ControllerBase
    {
        private readonly IPumpService _pumpService;
        private readonly IMapper _mapper;

        public PumpsController(IPumpService pumpService, IMapper mapper)
        {
            _pumpService = pumpService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pump>>> GetAllPumps()
        {
            var pumps = await _pumpService.Pumps.ToListAsync();
            var result = _mapper.Map<List<PumpDTO>>(pumps);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pump>> GetPump(int id)
        {
            var pump = await _context.Pumps.FindAsync(id);
            if (pump == null) return NotFound();
            var dto = _mapper.Map<PumpDTO>(pump);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<Pump>> CreatePump([FromBody] PumpCreateDTO createDto)
        {
            var pump = _mapper.Map<Pump>(createDto);
            _context.Pumps.Add(pump);
            await _context.SaveChangesAsync();

            var pumpDto = _mapper.Map<PumpDTO>(pump);
            return CreatedAtAction(nameof(GetPump), new { id = pump.Id }, pumpDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePump(int id, [FromBody] PumpCreateDTO updateDto)
        {
            var pump = await _context.Pumps.FindAsync(id);
            if (pump == null) return NotFound();

            _mapper.Map(updateDto, pump);

            await _context.SaveChangesAsync();
            return Ok(pump);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePump(int id)
        {
            var pump = await _context.Pumps.FindAsync(id);
            if (pump == null) return NotFound();

            _context.Pumps.Remove(pump);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
