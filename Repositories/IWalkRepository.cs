namespace UdemyProject.Repositories
{
    public interface IWalkRepository
    {
        Task<IEnumerable<Models.Domain.WalkDomain>> GetAllAsync();

        Task<Models.Domain.WalkDomain> GetAsync(Guid id);

        Task<Models.Domain.WalkDomain> AddAsync (Models.Domain.WalkDomain walkdomain);

        Task<Models.Domain.WalkDomain> UpdateAsync (Guid id, Models.Domain.WalkDomain walkdomain);

        Task<Models.Domain.WalkDomain> DeleteAsync (Guid id);
    }
}
