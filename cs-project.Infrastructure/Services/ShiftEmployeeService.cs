using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Infrastructure.Repositories;

namespace cs_project.Infrastructure.Services
{
    public class ShiftEmployeeService : IShiftEmployeeService
    {
        private readonly IShiftEmployeeRepository _shiftEmployeeRepository;
        private readonly IMapper _mapper;

        public ShiftEmployeeService(IShiftEmployeeRepository shiftEmployeeRepository, IMapper mapper)
        {
            _shiftEmployeeRepository = shiftEmployeeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ShiftEmployeeDTO>> GetEmployeesByShiftIdAsync(int shiftId)
        {
            var assignments = await _shiftEmployeeRepository.GetByShiftIdAsync(shiftId);
            return _mapper.Map<IEnumerable<ShiftEmployeeDTO>>(assignments);
        }

        public async Task<ShiftEmployeeDTO?> AddEmployeeToShiftAsync(int shiftId, int employeeId)
        {
            var existing = await _shiftEmployeeRepository.GetByShiftAndEmployeeIdAsync(shiftId, employeeId);
            if (existing != null) return null;

            var entity = new ShiftEmployee { ShiftId = shiftId, EmployeeId = employeeId };
            await _shiftEmployeeRepository.AddAsync(entity);
            await _shiftEmployeeRepository.SaveChangesAsync();
            return _mapper.Map<ShiftEmployeeDTO>(entity);
        }

        public async Task<bool> RemoveEmployeeFromShiftAsync(int shiftId, int employeeId)
        {
            var existing = await _shiftEmployeeRepository.GetByShiftAndEmployeeIdAsync(shiftId, employeeId);
            if (existing == null) return false;

            _shiftEmployeeRepository.Delete(existing);
            return await _shiftEmployeeRepository.SaveChangesAsync();
        }
    }
}

