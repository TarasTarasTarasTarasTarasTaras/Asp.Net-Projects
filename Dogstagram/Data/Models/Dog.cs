namespace Dogstagram.Server.Data.Models
{
    using Dogstagram.Server.Data.Models.Base;
    using System.ComponentModel.DataAnnotations;
    using static Data.Validation.Dog;

    public class Dog : DeletableEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
