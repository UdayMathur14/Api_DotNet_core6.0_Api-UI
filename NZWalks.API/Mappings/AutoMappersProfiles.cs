using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{
    public class AutoMappersProfiles :Profile
    {
        public AutoMappersProfiles()
        {
            CreateMap<Region,RegionDto>().ReverseMap();
            CreateMap<AddWalkRequestDto,Walk>().ReverseMap();
            CreateMap<Walk,WalkDto>().ReverseMap(); 

            CreateMap<Difficulty,DifficultyDto>().ReverseMap();

            CreateMap<UpdateWalkRequestDto , Walk>().ReverseMap();   
        }
    }
}
