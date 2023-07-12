using UdemyProject.Models.Domain;
using UdemyProject.Data;
using Microsoft.EntityFrameworkCore;

namespace UdemyProject.Repositories
{
    public class RegionRepository : IRegionRepository
    {

        private readonly DBContextClass _dBContextClasses;
        public RegionRepository(DBContextClass dBContextClasses) 
        {
            this._dBContextClasses = dBContextClasses;
        } 

        public async Task<IEnumerable<RegionDomain>> GetAllAsync()
        {
            return await _dBContextClasses.RegionTable.ToListAsync();
        }

        public async Task<RegionDomain> GetAsync(Guid UserID)
        {
            return await _dBContextClasses.RegionTable.FirstOrDefaultAsync(x => x.Id == UserID);
        }

        public async Task<RegionDomain> AddAsync(RegionDomain regionDomain)
        {
            regionDomain.Id = Guid.NewGuid();
            await _dBContextClasses.RegionTable.AddAsync(regionDomain);
            await _dBContextClasses.SaveChangesAsync();
            return regionDomain;
        }

        public async Task<RegionDomain> UpdateAsync(Guid UserID, RegionDomain regionDomain)
        {
            RegionDomain? existingregion = await _dBContextClasses.RegionTable.FirstOrDefaultAsync(x => x.Id == UserID);

            if (existingregion != null)
            {
                existingregion.Code = regionDomain.Code;
                existingregion.Name = regionDomain.Name;
                existingregion.Area = regionDomain.Area;
                existingregion.Lat = regionDomain.Lat;
                existingregion.Long = regionDomain.Long;
                existingregion.Population = regionDomain.Population;

                await _dBContextClasses.SaveChangesAsync();
            }

            return existingregion;
        }

        public async Task<RegionDomain> DeleteAsync(Guid UserId)
        {
            RegionDomain? regionDomain = await _dBContextClasses.RegionTable.FirstOrDefaultAsync(x => x.Id == UserId);

            if (regionDomain != null)
            {
                _dBContextClasses.RegionTable.Remove(regionDomain);
                await _dBContextClasses.SaveChangesAsync();
            }
            
            return regionDomain;
        }

    }
}
