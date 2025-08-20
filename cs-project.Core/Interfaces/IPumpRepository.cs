using cs_project.Core.Entities;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Repositories
{
    public interface IPumpRepository
    {
        Task<(IEnumerable<Pump> Items, int TotalCount)> QueryPumpsAsync(PagingQueryParameters query);
        Task<IEnumerable<Pump>> GetAllAsync();
        Task<Pump?> GetByIdAsync(int id);
        Task AddAsync(Pump pump);
        void Update(Pump pump);
        void Delete(Pump pump);
        Task<bool> SaveChangesAsync();
    }
}
