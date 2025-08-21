using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Core.Models;
using cs_project.Infrastructure.Repositories;

namespace cs_project.Infrastructure.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<EmployeeDTO>> GetEmployeeAsync(PagingQueryParameters query)
        {
            var (entities, total) = await _employeeRepository.QueryEmployeesAsync(query);
            var dtoList = _mapper.Map<IEnumerable<EmployeeDTO>>(entities);

            return new PagedResult<EmployeeDTO>
            {
                Items = dtoList,
                TotalCount = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EmployeeDTO>>(employees);
        }

        public async Task<EmployeeDTO?> GetEmployeeByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            return employee == null ? null : _mapper.Map<EmployeeDTO>(employee);
        }

        public async Task<EmployeeDTO> CreateEmployeeAsync(EmployeeCreateDTO dto)
        {
            var employee = _mapper.Map<Employee>(dto);
            await _employeeRepository.AddAsync(employee);
            await _employeeRepository.SaveChangesAsync();
            return _mapper.Map<EmployeeDTO>(employee);
        }

        public async Task<bool> UpdateEmployeeAsync(int id, EmployeeCreateDTO dto)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return false;

            _mapper.Map(dto, employee);
            _employeeRepository.Update(employee);
            return await _employeeRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return false;

            _employeeRepository.Delete(employee);
            return await _employeeRepository.SaveChangesAsync();
        }
    }
}
