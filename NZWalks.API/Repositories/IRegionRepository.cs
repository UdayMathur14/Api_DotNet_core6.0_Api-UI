using Microsoft.AspNetCore.Http.Metadata;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllRegionsAsync();

        //Region? means h ki yeh null return kr skti h 
        Task<Region?> GetByIdAsync(Guid id);

        Task<Region> CreateAsync(Region region);

        Task<Region?> UpdateAsync(Guid id , Region region);

        Task<Region?> DeleteAsync(Guid id);




    }
}


//what is repository pattern 
//1 - design pattern to seprate the data access layer from the application 
//2 - provides interface without exposing implemention 
//3 - helps create abstrcation 

// it is responsible for performing the crud operations on the data store 
//data flow 

//controller - repository - > database 

//phele dbcontext class thi usko inject krte the controller ke ander and then wo controller tha jo ki directly talking to the database using the db context 

// yeh ek layer bna deta h controller and database ke beech m , then repositary handle kregi saare opertaion 

// then controller user kregera repository instead of the db context 

//ab controller ke pass koi awarness nhi hogi ki kya kya chl rha h dbcontext m 


//benefits of repository pattern :
//1 - Decoupling 
//2 - consistency 
// performance 
