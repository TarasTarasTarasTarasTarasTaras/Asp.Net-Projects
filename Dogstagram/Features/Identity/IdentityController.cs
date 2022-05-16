namespace Dogstagram.Server.Features.Identity
{
    using Dogstagram.Server.Data.Models;
    using Dogstagram.Server.Features.Identity.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using System.Threading.Tasks;

    public class IdentityController : ApiController
    {
        private readonly UserManager<User> userManager;
        private readonly ApplicationSettings appSettings;
        private readonly IIdentityService identityService;

        public IdentityController(UserManager<User> userManager, IIdentityService identityService, IOptions<ApplicationSettings> appSettings)
        {
            this.userManager = userManager;
            this.identityService = identityService;
            this.appSettings = appSettings.Value;
        }

        [HttpPost]
        [Route(nameof(Register))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Register(RegisterRequestModel model)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
                return Ok("Created a new user");
            return BadRequest(result.Errors);
        }

        [HttpPost]
        [Route(nameof(Login))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user is null)
                return Unauthorized();

            var passwordValid = await userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
                return Unauthorized();

            var token = this.identityService.GenerateJwtToken(user.Id, user.UserName, this.appSettings.Secret);

            return new LoginResponseModel
            {
                Token = token
            };
        }
    }
}
