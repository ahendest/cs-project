using cs_project.Core.Entities;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Repositories
{
    public interface IStationRepository
    {
        Task<(IEnumerable<Station> Items, int TotalCount)> QueryStationsAsync(PagingQueryParameters query);
        Task<IEnumerable<Station>> GetAllAsync();
        Task<Station?> GetByIdAsync(int id);
        Task AddAsync(Station station);
        void Update(Station station);
        void Delete(Station station);
        Task<bool> SaveChangesAsync();
    }
}
