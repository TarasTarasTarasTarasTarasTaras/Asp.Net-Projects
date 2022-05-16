namespace Dogstagram.Server.Features.Search
{
    using Dogstagram.Server.Data;
    using Dogstagram.Server.Features.Search.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class SearchService : ISearchService
    {
        private readonly DogstagramDbContext context;

        public SearchService(DogstagramDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ProfileSearchResponseModel>> Profiles(string query)
        {
            return await this.context
                .Users
                .Where(u => u.UserName.ToLower().Contains(query.ToLower())|| u.Profile.Name.ToLower().Contains(query.ToLower()))
                .Select(u => new ProfileSearchResponseModel
                {
                    UserId = u.Id,
                    UserName = u.UserName,
                    Name = u.Profile.Name,
                    ProfilePhotoUrl = u.Profile.MainPhotoUrl
                })
                .ToListAsync();
        }
    }
}
