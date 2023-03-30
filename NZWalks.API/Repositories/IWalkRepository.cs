using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);

        Task<List<Walk>> GetAllAsync(string? filterOn = null , string? filterQuery=null ,
            string? sortBy =null, bool isAscending = true ,int pageNumber = 1 , int pageSize = 1000 );

        Task<Walk?> GetByIdAsync(Guid id);

        Task<Walk?> UpdatebyIdAsync(Guid id , Walk walk);

        Task<Walk?> DeleteByIdAsync(Guid id);
    }
}


//navigation property used to fetch the data from the database 
//typically define in the form of object 

//1 - allow to navigate from one entity to another
//walks domain model will have a region navigation property 