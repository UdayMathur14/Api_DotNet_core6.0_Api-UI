using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //use authorize attribute to block unauthenicated users
    //after writing this our region apis are not working because they need a authorize user to use it so if you test 
    //region api is hows 401 error which is a unorized user , so to overcome this so create some users for authentciate 
    //and then we can use these apis
   // [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly NZwalksDbContext dbcontext;
        private readonly IMapper mapper;

        public IRegionRepository RegionRepository { get; }

        public RegionsController(NZwalksDbContext dbcontext , IRegionRepository regionRepository,IMapper mapper)
        {
            this.dbcontext = dbcontext;
            RegionRepository = regionRepository;
            this.mapper = mapper;
        }
        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    //this way we can get the hard coded values in the regions and print in api 

        //    //var regions = new List<Region>
        //    //{
        //    //    new Region
        //    //    {
        //    //        Id = Guid.NewGuid(),
        //    //        Name = "Name",
        //    //        Code = "1",
        //    //        RegionImageUrl = "1",

        //    //    },
        //    //    new Region
        //    //    {
        //    //        Id = Guid.NewGuid(),
        //    //        Name = "Name",
        //    //        Code = "1",
        //    //        RegionImageUrl = "1",

        //    //    },

        //    //};
        //    //return Ok(regions);

        //    // without the use of the in memory , but use databse 

        //    //hmne ek dbcontext ki class banai aur uske ander hmne ek table name declare kiya hua tah 
        //    //region hmne usses ccess kiya ur saari list nikaal li usme m and api ko ddedi
        //    var regions = dbcontext.Regions.ToList();
        //    return Ok(regions);
        //}


        //fetching the information with the help of Dtos
        [HttpGet]
        [Authorize(Roles ="Reader")]
        //we want a reader authorise member to access this function 
        public async Task<IActionResult> GetAll()
        {
            //with the help of Interface known as IregionRepository 
            
            //var regionsDomain = await RegionRepository.GetAllRegionsAsync();










            //get data from databse - domain model 
            //map domain model to dtos
            // return dto 

            var regions = await dbcontext.Regions.ToListAsync();

            var regionsDto = new List<RegionDto>();
            //without the use of automapper

            //foreach (var region in regions)
            //{
            //    regionsDto.Add(new RegionDto()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        RegionImageUrl = region.RegionImageUrl
            //    });
            //}

            //with the use of auto mapper 
           var region =  mapper.Map<List<RegionDto>>(regions);

            //return Ok(region);
            return Ok(mapper.Map<List<RegionDto>>(regions));

        }


        //get single region or get region by id with the help of databse 
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]

        public async Task<IActionResult> GetbyId([FromRoute] Guid id)
        {
           
            // this is with the help of Interface 
            //var regionDomain = await RegionRepository.GetById(id);
            //return Ok(regionDomain);

            //another method of this is 
            //var regions = dbcontext.Regions.Find(id);







            //normal with the help of dbcontext

            var regions = await dbcontext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (regions == null)
            {
                return NotFound();
            }
            return Ok(regions);


        }

        [HttpPost]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //return a boolean value , hmne addregion dto m jaake required field rakha and then usme define kra ki minimum 3 char 
            // adn maximum 3 hone chchiye model state ussi ko laagu krta h ki check krega ki addregiondto m jo bhi hm cheez 
            //daal rhe h usko validate kro 

            //check krta h ki if my model is valid or not 
            if (ModelState.IsValid)
            {



                //Map or convert the dto to Domain model 
                //use domain model to create Region

                var regionDomainModel = new Region
                {
                    Code = addRegionRequestDto.Code,
                    Name = addRegionRequestDto.Name,
                    RegionImageUrl = addRegionRequestDto.RegionImageUrl
                };

                //WITH THE HELP OF Interface
                //regionDomainModel = await RegionRepository.CreateAsync(regionDomainModel);
                //return CreatedAtAction(nameof(GetbyId), new { id = regionDto.Id }, regionDto);
                //AbandonedMutexException then same in lower 






                //use dbcontext to add in the datatbse 
                await dbcontext.Regions.AddAsync(regionDomainModel);
                await dbcontext.SaveChangesAsync();

                //Map domain domain model back to dto 

                var regionDto = new RegionDto
                {
                    Id = regionDomainModel.Id,
                    Code = regionDomainModel.Code,
                    Name = regionDomainModel.Name,
                    RegionImageUrl = regionDomainModel.RegionImageUrl
                };
                //post method does not return an ok responce 
                return CreatedAtAction(nameof(GetbyId), new { id = regionDto.Id }, regionDto);
            }
            else
            {
                return BadRequest(ModelState);
            }

        }


        //update region

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult>Update([FromRoute] Guid id , [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //call the function with the help of interface , hmne dto ko domain model m convert krna padega
            //map dto to domain model 
            //            var regionDomainModel = new Region
            //            {
            //                Code = updateRegionRequestDto.Code,
            //                Name = updateRegionRequestDto.Name,
            //                RegionImageUrl = updateRegionRequestDto.RegionImageUrl
            //            };
            //{            regionDomainModel = await RegionRepository.UpdateAsync(id, regionDomainModel);
            //            //check if region exists


            if (ModelState.IsValid)
            {

                var regionDomainModel = dbcontext.Regions.FirstOrDefault(x => x.Id == id);
                if (regionDomainModel == null)
                {
                    return NotFound();
                }
                //map dto to domain model 

                regionDomainModel.Code = updateRegionRequestDto.Code;
                regionDomainModel.Name = updateRegionRequestDto.Name;
                regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;


                await dbcontext.SaveChangesAsync();

                var regionDto = new RegionDto
                {
                    Id = regionDomainModel.Id,
                    Code = regionDomainModel.Code,
                    Name = regionDomainModel.Name,
                    RegionImageUrl = regionDomainModel.RegionImageUrl
                };

                return Ok(regionDto);

            }
            else
            {
                return BadRequest(ModelState);
            }

            
        }


        [HttpDelete]
        [Route("{id:Guid}")]
        //both reader or writer can access this in the postman
        [Authorize(Roles = "Writer,Reader")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {

            //with the help of interface 
            //bs yhi statemnet likhni h baaki neeche mapping ho rhi h remove and save changes toh sql region ke ander hi ho rha h toh need nhi h yha likhne ki 
            //var regionDomainModel = await RegionRepository.DeleteAsync(id);








            var regionDomainModel= await  dbcontext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (regionDomainModel == null)
            {
                return NotFound();  
            }

            //delete region 
            dbcontext.Regions.Remove(regionDomainModel);
            await dbcontext.SaveChangesAsync();

            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            //return the deleted region back 
            //map a domain model dto 

            return Ok(regionDomainModel);
        
        
        }

       
    }
}
