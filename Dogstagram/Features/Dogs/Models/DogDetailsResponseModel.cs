namespace Dogstagram.Server.Features.Dogs.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DogDetailsResponseModel : DogListingResponseModel
    {
        public string Description { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }
    }
}
