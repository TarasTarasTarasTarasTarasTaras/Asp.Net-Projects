namespace Dogstagram.Server.Features.Dogs
{
    using Dogstagram.Server.Data.Models;
    using Dogstagram.Server.Features.Dogs.Models;
    using Dogstagram.Server.Infrastructure;
    using Dogstagram.Server.Infrastructure.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class DogsController : ApiController
    {
        private readonly IDogService dogService;
        private readonly ICurrentUserService currentUser;

        public DogsController(IDogService dogService, ICurrentUserService currentUser)
        {
            this.dogService = dogService;
            this.currentUser = currentUser;
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<DogListingResponseModel>> Mine()
        {
            string userId = currentUser.GetId();
            if (userId is null) return null;
            string userName = currentUser.GetUserName();
            return await dogService.ByUser(userId, userName);
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<DogDetailsResponseModel>> Details(int id)
        {
            var dog = await dogService.ByUserDetails(id);

            if (dog is null) return NotFound();

            return dog;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create(CreateDogRequestModel model)
        {
            var userId = this.currentUser.GetId();

            var id = await this.dogService.Create(model.ImageUrl, model.Description, userId);

            return Created(nameof(Create), id);
        }

        [Authorize]
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Dog>> Update(int id, UpdateDogRequestModel model)
        {
            var userId = this.currentUser.GetId();

            var result = await this.dogService.Update(id, model.Description, userId);

            if (result.Failed) return BadRequest(result.Error);

            return Ok();
        }

        [Authorize]
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<DogDetailsResponseModel>> Delete(int id)
        {
            var userId = this.currentUser.GetId();

            var result = await this.dogService.Delete(id, userId);
            if (result.Failed) return BadRequest(result.Error);

            return Ok("Deleted");
        }
    }
}
