using Microsoft.EntityFrameworkCore;
using UdemyProject.Data;
using UdemyProject.Models.Domain;

namespace UdemyProject.Repositories
{
    public class DifficultyRepository : IDifficultyRepository
    {
        private readonly DBContextClass _dbContextClasses;
        public DifficultyRepository(DBContextClass dBContextClasses)
        {
            _dbContextClasses = dBContextClasses;
        }

        public async Task<IEnumerable<DifficultyDomain>> GetAllAsync()
        {
            return await _dbContextClasses.DifficultyTable.ToListAsync();
        }

        public async Task<DifficultyDomain> GetAsync(Guid Userid)
        {
            return await _dbContextClasses.DifficultyTable.FirstOrDefaultAsync(x => x.Id == Userid);
                
        }

        public async Task<DifficultyDomain> AddAsync(DifficultyDomain DifficultyDomain)
        {
            DifficultyDomain.Id = Guid.NewGuid();
            await _dbContextClasses.DifficultyTable.AddAsync(DifficultyDomain);
            await _dbContextClasses.SaveChangesAsync();
            return DifficultyDomain;
        }

        public async Task<DifficultyDomain> UpdateAsync(Guid Userid, DifficultyDomain DifficultyDomain)
        {
            DifficultyDomain? existingDifficulty = await _dbContextClasses.DifficultyTable.FindAsync(Userid);

            if (existingDifficulty != null)
            {
                existingDifficulty.Code = DifficultyDomain.Code;
            }

            await _dbContextClasses.SaveChangesAsync();
            return existingDifficulty;
        }

        public async Task<DifficultyDomain> DeleteAsync(Guid Userid)
        {
            DifficultyDomain? DifficultyDomain = await _dbContextClasses.DifficultyTable.FindAsync(Userid);

            if (DifficultyDomain != null)
            {
                _dbContextClasses.DifficultyTable.Remove(DifficultyDomain);
                _dbContextClasses.SaveChanges();
            }
            return DifficultyDomain;
        }
    }
}
 