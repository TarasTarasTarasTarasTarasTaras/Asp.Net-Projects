namespace Dogstagram.Server.Features.Profiles
{
    using Dogstagram.Server.Features.Follows;
    using Dogstagram.Server.Features.Profiles.Models;
    using Dogstagram.Server.Infrastructure.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class ProfilesController : ApiController
    {
        private readonly IProfileService _profiles;
        private readonly IFollowService _follows;
        private readonly ICurrentUserService _currentUser;

        public ProfilesController(IProfileService profiles, ICurrentUserService currentUser = null, IFollowService follows = null)
        {
            _profiles = profiles;
            _currentUser = currentUser;
            _follows = follows;
        }

        [HttpGet]
        [Authorize]
        public async Task<ProfileResponseModel> Mine()
        {
            return await this._profiles.ByUser(_currentUser.GetId(), allInformation: true);
        }
         
        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public async Task<ProfileResponseModel> Details(string id)
        {
            string userId = this._currentUser.GetId();
            bool includeAllInformation = await this._follows.IsFollower(id, userId);

            if (!includeAllInformation)
            {
                includeAllInformation = await this._profiles.IsPublic(id);
            }

            return await this._profiles.ByUser(id, includeAllInformation);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> Update(UpdateProfileRequestModel model)
        {
            var userId = this._currentUser.GetId();

            var result = await this._profiles.Update(
                userId,
                model.Email,
                model.UserName,
                model.Name,
                model.MainPhotoUrl,
                model.WebSite,
                model.Biography,
                model.Gender,
                model.IsPrivate);

            if (result.Failed)
                return BadRequest(result.Error);

            return Ok();
        }
    }
}