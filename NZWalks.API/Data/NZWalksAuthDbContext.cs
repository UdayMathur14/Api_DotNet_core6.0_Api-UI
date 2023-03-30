using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options): base (options) 
        {

        }
        //check the auth controller 1st 
        //is the time to create the roles and we want to seed some roles in the database so that when we do a user 
        //user registration , we can supply these roles and create a user with these roles .

        //seed some data using entity framework 
                                                //yeh iss builder object ke ander jayega 
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

           var readerRoleId = "e16e4175-6416-4720-b98d-9970be217e66";
           var writerRoleId = "ec193f1f-a8fd-4a03-bb94-5950d6773b41";

           var roles = new List<IdentityRole>
           {
                new IdentityRole
                {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper()
                },
                new IdentityRole
                {
                    Id = writerRoleId,
                    ConcurrencyStamp = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper()
                }

           };
            //now we have to seed this inside our builder over here in the function 

            //so when we run entity framework core migration , now it will see this data that we have to inject these.
            //jo 2 upper h 

            //if the roles don't exist in the database , this entity framework core migration will add or seed this data 
            //into the database .

            //migration krega to dikhat ayegi kyuki phele dbcontext ho chuka h so we write like this 
            //add-migration "dsd" -context "nzwalksauthdbcontext"
            //same update m bhi , hme specify krna hoga ki hme konsa update krna h with -context after command in the ""


            //one more piece of setup is left after the core migration and it is for identity and then we can switch
            //to creating creating the controllers 
            builder.Entity<IdentityRole>().HasData(roles);
        }

    }
}

