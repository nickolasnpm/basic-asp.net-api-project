using UdemyProject.Models.Domain;

namespace UdemyProject.Repositories
{
    public interface IRegionRepository
    {
        Task<IEnumerable<RegionDomain>> GetAllAsync();
        Task<RegionDomain> GetAsync(Guid Id);
        Task<RegionDomain> AddAsync(RegionDomain regionDomain);
        Task<RegionDomain> DeleteAsync(Guid Id);
        Task<RegionDomain> UpdateAsync(Guid id, RegionDomain regionDomain);
    }
}
