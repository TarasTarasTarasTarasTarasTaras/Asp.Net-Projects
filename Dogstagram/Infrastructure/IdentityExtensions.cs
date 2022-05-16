namespace Dogstagram.Server.Infrastructure
{
    using System.Linq;
    using System.Security.Claims;

    public static class IdentityExtensions
    {
        // to get the user id when creating a new dog and get dogs
        public static string GetId(this ClaimsPrincipal user)
        {
            return user.Claims.First(u => u.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.Claims.First(u => u.Type == ClaimTypes.Name)?.Value;
        }
    }
}
