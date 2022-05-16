namespace Dogstagram.Server.Features
{
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : ApiController
    {
        [HttpGet]
        [Route(nameof(Get))]
        public ActionResult Get()
        {
            return Ok("Works");
        }
    }
}
