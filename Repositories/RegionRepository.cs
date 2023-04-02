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
            // return all data in the RegionTable in the form of list to RegionsController
        }

        public async Task<RegionDomain> GetAsync(Guid UserID)
        {
            return await _dBContextClasses.RegionTable.FirstOrDefaultAsync(x => x.Id == UserID);
            // return one single data in the RegionTable that match the UserID
        }

        public async Task<RegionDomain> AddAsync(RegionDomain regionDomain)
        {
            regionDomain.Id = Guid.NewGuid();
            // new Guid Id is generated for this new object addition

            await _dBContextClasses.RegionTable.AddAsync(regionDomain);
            // Add regionDomain object in the AddAsync method into the RegionTable

            await _dBContextClasses.SaveChangesAsync();
            // Make sure the change is saved in the DBContext

            return regionDomain;
            // Return the newly-added regionDomain
            // the return data is based on the AddRegionRequest 
        }

        public async Task<RegionDomain> UpdateAsync(Guid UserID, RegionDomain regionDomain)
        {
            RegionDomain? existingregion = await _dBContextClasses.RegionTable.FirstOrDefaultAsync(x => x.Id == UserID);
            // return one single data in the RegionTable that match the UserID

            if (existingregion == null)
            {
                return null;
            }
            // continue the flow if the existingregion != null

            existingregion.Code = regionDomain.Code;
            existingregion.Name = regionDomain.Name;
            existingregion.Area = regionDomain.Area;
            existingregion.Lat = regionDomain.Lat;
            existingregion.Long = regionDomain.Long;
            existingregion.Population = regionDomain.Population;

            // map the data from regionDomain data (carried forward from user input) to existingregion 

            await _dBContextClasses.SaveChangesAsync();
            //  Make sure the change is saved in the DBContext

            return existingregion;
            // Return the newly-updated regionDomain
        }

        public async Task<RegionDomain> DeleteAsync(Guid UserId)
        {
            RegionDomain? regionDomain = await _dBContextClasses.RegionTable.FirstOrDefaultAsync(x => x.Id == UserId);
            // return one single data in the RegionTable that match the UserID

            if (regionDomain == null)
            {
                return null;
            }
            else
            {
                _dBContextClasses.RegionTable.Remove(regionDomain);
                // If the regionDomain with the UserID is not null, remove it from the database
            }

            await _dBContextClasses.SaveChangesAsync();
            // save the change made to the database

            return regionDomain;
            // return the already removed regionDomain to RegionsController
        }

    }
}
