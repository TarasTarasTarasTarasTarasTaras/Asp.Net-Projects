namespace Dogstagram.Server.Features.Profiles.Models
{
    public class PublicProfileResponseModel : ProfileResponseModel
    {
        public string WebSite { get; set; }

        public string Biography { get; set; }

        public string Gender { get; set; }
    }
}
