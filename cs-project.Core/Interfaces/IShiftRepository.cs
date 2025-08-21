using cs_project.Core.Entities;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Repositories
{
    public interface IShiftRepository
    {
        Task<(IEnumerable<Shift> Items, int TotalCount)> QueryShiftsAsync(PagingQueryParameters query);
        Task<IEnumerable<Shift>> GetAllAsync();
        Task<Shift?> GetByIdAsync(int id);
        Task AddAsync(Shift shift);
        void Update(Shift shift);
        void Delete(Shift shift);
        Task<bool> SaveChangesAsync();
    }
}
