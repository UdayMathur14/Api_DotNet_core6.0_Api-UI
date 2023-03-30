using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class InMemoryRegionRepository 
       // :IRegionRepository
    {
        public async Task<List<Region>> GetAllRegionsAsync()
        {
            return new List<Region>
            {
                new Region()
                {
                    Id = Guid.NewGuid(),
                    Code = "SAM",
                    Name = "Sameer Region Name",
                }
            };
        }
    }

}


//we use this class beacuse we have to use in memory memeory previously we use sql 