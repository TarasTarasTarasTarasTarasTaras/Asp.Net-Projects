namespace Dogstagram.Server.Features.Profiles
{
    using Dogstagram.Server.Data;
    using Dogstagram.Server.Data.Models;
    using Dogstagram.Server.Features.Profiles.Models;
    using Dogstagram.Server.Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProfileService : IProfileService
    {
        private readonly DogstagramDbContext context;

        public ProfileService(DogstagramDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> IsPublic(string userId)
        {
            return !await this.context
                .Profiles
                .Where(u => u.UserId == userId)
                .Select(u => u.IsPrivate)
                .FirstOrDefaultAsync();
        }

        public async Task<ProfileResponseModel> ByUser(string userId, bool allInformation)
        {
            try
            {
                return await this.context
                    .Users
                    .Where(u => u.Id == userId)
                    .Select(u => allInformation
                    ? new PublicProfileResponseModel
                    {
                        Name = u.Profile.Name,
                        MainPhotoUrl = u.Profile.MainPhotoUrl,
                        Biography = u.Profile.Biography,
                        IsPrivate = u.Profile.IsPrivate,
                        WebSite = u.Profile.WebSite,
                        Gender = u.Profile.Gender.ToString()
                    }
                    : new ProfileResponseModel
                    {
                        Name = u.Profile.Name,
                        MainPhotoUrl = u.Profile.MainPhotoUrl,
                        IsPrivate = u.Profile.IsPrivate
                    })
                    .FirstOrDefaultAsync();
            }
            catch
            {
                return allInformation ? new PublicProfileResponseModel() : new ProfileResponseModel();
            }
        }

        public async Task<Result> Update(
            string userId,
            string email,
            string userName,
            string name,
            string mainPhotoUrl,
            string webSite,
            string biography,
            Gender gender,
            bool isPrivate)
        {
            var user = await this.context
                .Users
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null) return "User does not exist";

            if (user.Profile is null) user.Profile = new Profile();

            Result result = await this.ChangeEmail(user, userId, email);
            if (result.Failed) return result;

            result = await this.ChangeUserName(user, userId, userName);
            if (result.Failed) return result;

            this.ChangeProfile(user.Profile, name, mainPhotoUrl, webSite, biography, gender, isPrivate);

            await this.context.SaveChangesAsync();

            return true;
        }


        private async Task<Result> ChangeEmail(User user, string userId, string email)
        {
            if (!string.IsNullOrWhiteSpace(email) && user.Email != email)
            {
                var emailExist = await context
                    .Users.AnyAsync(u => u.Id != userId && u.Email == email);

                if (emailExist)
                    return "A user with this email already exists";

                user.Email = email;
            }
            return true;
        }

        private async Task<Result> ChangeUserName(User user, string userId, string userName)
        {
            if (!string.IsNullOrWhiteSpace(userName) && user.UserName != userName)
            {
                var userNameExist = await context
                    .Users.AnyAsync(u => u.Id != userId && u.UserName == userName);

                if (userNameExist)
                    return "A user with this username already exists";

                user.Email = userName;
            }
            return true;
        }

        private void ChangeProfile(
            Profile profile,
            string name,
            string mainPhotoUrl, 
            string webSite, 
            string biography,
            Gender gender,
            bool isPrivate)
        {
            if(profile.Name != name)
                profile.Name = name;

            if(profile.MainPhotoUrl != mainPhotoUrl)
                profile.MainPhotoUrl = mainPhotoUrl;

            if(profile.WebSite != webSite)
                profile.WebSite = webSite;

            if(profile.Biography != biography)
                profile.Biography = biography;

            if(profile.Gender != gender)
                profile.Gender = gender;

            if(profile.IsPrivate != isPrivate)
                profile.IsPrivate = isPrivate;
        }
    }
}