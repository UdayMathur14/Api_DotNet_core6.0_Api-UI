using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZwalksDbContext dbContext;

        public SQLWalkRepository(NZwalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {

            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

      

        public async Task<List<Walk>> GetAllAsync( string? filterOn, string? filterQuery , 
            string? sortBy = null , bool isAscending = true , int pageNumber = 1, int pageSize = 1000)
        {

            var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //apply filtering 
            if(string.IsNullOrWhiteSpace(filterOn) ==false && string.IsNullOrWhiteSpace(filterQuery) == false  )
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));

                }
            }

            //sorting 

            if(string.IsNullOrWhiteSpace(sortBy)==false )
            {
                if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks= isAscending ? walks.OrderBy(x=>x.Name) :walks.OrderByDescending(x=>x.Name);  
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm):walks.OrderByDescending(x=>x.LengthInKm);
                }
            }


            //Pagination 

            var skipResult = (pageNumber - 1) * pageSize;

            //but for pagination we use some different 
            return await walks.Skip(skipResult).Take(pageSize).ToListAsync();
            //this is again for the same in sorting and filtering 
           // return await walks.ToListAsync();
            //yeh normal output h api ke liye but when we use filter so i have to change my query 
            //return  await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async  Task<Walk?> GetByIdAsync(Guid id)
        {
            return await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdatebyIdAsync(Guid id, Walk walk)
        {
            var existingWalk = await  dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }

            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.RegionId = walk.RegionId;

            await dbContext.SaveChangesAsync();

            return existingWalk;
            



        }

        public async Task<Walk?> DeleteByIdAsync(Guid id)
        {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x=>x.Id == id);   
            if(existingWalk == null)
            {
                return null;
            }
             dbContext.Walks.Remove(existingWalk);
            await dbContext.SaveChangesAsync();
            return existingWalk;

        }

        //public Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool? isAscending = true)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
