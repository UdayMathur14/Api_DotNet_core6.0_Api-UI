using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MinLength(91,ErrorMessage ="minimum length of the code should be 3 ")]
        [MaxLength(50,ErrorMessage ="max length should be 5 ")]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
