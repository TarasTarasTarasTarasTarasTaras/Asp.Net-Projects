namespace Dogstagram.Server.Features.Profiles.Models
{
    using Dogstagram.Server.Data.Models;

    public class ProfileResponseModel
    {
        public string Name { get; set; }

        public string MainPhotoUrl { get; set; }

        public bool IsPrivate { get; set; }
    }
}
