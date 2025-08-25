using cs_project.Core.DTOs;
using cs_project.Core.Models;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace cs_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAllEmployees(CancellationToken ct)
        {
            var employees = await _employeeService.GetAllEmployeesAsync(ct);
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id, CancellationToken ct)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id, ct);
            if (employee == null) return NotFound();
            return Ok(employee);
        }

        [HttpGet("query")]
        public async Task<ActionResult<PagedResult<EmployeeDTO>>> QueryEmployees([FromQuery] PagingQueryParameters query, CancellationToken ct)
        {
            var result = await _employeeService.GetEmployeeAsync(query, ct);
            Response.Headers.Append("X-Total-Count", result.TotalCount.ToString());

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDTO>> CreateEmployee([FromBody] EmployeeCreateDTO createDto, CancellationToken ct)
        {
            _logger.LogInformation("Employee Created!");
            var employeeDto = await _employeeService.CreateEmployeeAsync(createDto, ct);
            return CreatedAtAction(nameof(GetEmployee), new { id = employeeDto.Id }, employeeDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeCreateDTO updateDto, CancellationToken ct)
        {
            var updated = await _employeeService.UpdateEmployeeAsync(id, updateDto, ct);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id, CancellationToken ct)
        {
            var deleted = await _employeeService.DeleteEmployeeAsync(id, ct);
            return deleted ? NoContent() : NotFound();
        }
    }
}

