using cs_project.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cs_project.Infrastructure.Repositories
{
    public interface IPumpRepository
    {
        Task<IEnumerable<Pump>> GetAllAsync();
        Task<Pump?> GetByIdAsync(int id);
        Task AddAsync(Pump pump);
        void Update(Pump pump);
        void Delete(Pump pump);
        Task<bool> SaveChangesAsync();
    }
}
