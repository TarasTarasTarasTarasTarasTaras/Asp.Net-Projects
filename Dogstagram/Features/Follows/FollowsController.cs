namespace Dogstagram.Server.Features.Follows
{
    using Dogstagram.Server.Features.Follows.Models;
    using Dogstagram.Server.Infrastructure.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class FollowsController : ApiController
    {
        private readonly IFollowService _followService;
        private readonly ICurrentUserService _currentUser;


        public FollowsController(IFollowService followService, ICurrentUserService currentUser)
        {
            _followService = followService;
            _currentUser = currentUser;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Follow(FollowRequestModel model)
        {
            var followerId = this._currentUser.GetId();
            Result result = await this._followService.Follow(model.UserId, followerId);

            if (result.Failed)
                return BadRequest(result.Error);

            return Ok();
        }
    }
}