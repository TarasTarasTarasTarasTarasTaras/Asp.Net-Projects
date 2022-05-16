namespace Dogstagram.Server.Features.Profiles
{
    using Dogstagram.Server.Data.Models;
    using Dogstagram.Server.Features.Profiles.Models;
    using Dogstagram.Server.Infrastructure.Services;
    using System.Threading.Tasks;

    public interface IProfileService
    {
        Task<ProfileResponseModel> ByUser(string userId, bool allInformation);
        
        Task<bool> IsPublic(string userId);

        Task<Result> Update(
            string userId,
            string email,
            string userName,
            string name,
            string mainPhotoUrl,
            string webSite,
            string biography,
            Gender gender,
            bool isPrivate);
    }
}
