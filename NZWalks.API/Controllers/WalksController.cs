using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }


        [HttpPost]
        //add this attribute to not check model.isvalid in the function, now are checking this method  
        [ValidateModel]
        //create walk 

        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            if (ModelState.IsValid)
            {
                //map dto to domain model 
                var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

                await walkRepository.CreateAsync(walkDomainModel);



                //map domain model to dto 


                return Ok(mapper.Map<WalkDto>(walkDomainModel));
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpGet]
        //filter from the query 
        //get:/api/walks?filterOn=Name&filter& sortby(is the columnname).isascending = true

        //[FromQuery] string? filterOn, [FromQuery] string? filterQuery = this is for the filtering and then the 
        //page no = 1 and the page size = 10 

        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery ,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000 )
        {

            //we can use filter when we run our api there are two options 
            //1 - search by name because we pass the name in the repository
            //sorting and filtering get methods p hi hoga
            var a  = await walkRepository.GetAllAsync(filterOn , filterQuery , sortBy , isAscending?? true , pageNumber , pageSize);
            return Ok(mapper.Map<List<WalkDto>>(a));
        }



        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetBYID([FromRoute] Guid id)
        {
            var walkDomainmodel = await walkRepository.GetByIdAsync(id);

            if(walkDomainmodel == null)
            {
                return NotFound();  
            }

            //map domain model to dto 

            return Ok(mapper.Map<WalkDto>(walkDomainmodel));
        }

        [HttpPut]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Update([FromRoute] Guid id , UpdateWalkRequestDto updateWalkRequestDto)
        {
            if (ModelState.IsValid)
            {
                var walkDomainmodel = mapper.Map<Walk>(updateWalkRequestDto);
                walkDomainmodel = await walkRepository.UpdatebyIdAsync(id, walkDomainmodel);

                if (walkDomainmodel == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<WalkDto>(walkDomainmodel));
            }
            else
            {
                return BadRequest(ModelState);
            }


        }

        //delete a walk 

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletetedmodel = await walkRepository.DeleteByIdAsync(id);
            if(deletetedmodel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDto>(deletetedmodel));
        }
    }
}
