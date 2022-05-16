namespace Dogstagram.Server.Features.Dogs.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Data.Validation.Dog;

    public class CreateDogRequestModel
    {
        [Required]
        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(MaxDescriptionLength)]
        public string Description { get; set; }
    }
}
