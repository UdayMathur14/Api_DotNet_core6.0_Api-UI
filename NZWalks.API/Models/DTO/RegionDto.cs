namespace NZWalks.API.Models.DTO
{
    public class RegionDto
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
//
//AutoMapper

//1 - object to object mapping 
//2 - Simplication
// 3-MAP BETWEEN THE DTO AND DOMAIN MODEL
// 4 - Quite powerful apart from just simple object to object mapping 
