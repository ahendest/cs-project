using cs_project.Core.Entities;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Repositories
{
    public interface ITankRepository
    {
        Task<(IEnumerable<Tank> Items, int TotalCount)> QueryTanksAsync(PagingQueryParameters query);
        Task<IEnumerable<Tank>> GetAllAsync();
        Task<Tank?> GetByIdAsync(int id);
        Task AddAsync(Tank tank);
        void Update(Tank tank);
        void Delete(Tank tank);
        Task<bool> SaveChangesAsync();
    }
}
