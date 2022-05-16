namespace Dogstagram.Server.Features.Search
{
    using Dogstagram.Server.Features.Search.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISearchService
    {
        Task<IEnumerable<ProfileSearchResponseModel>> Profiles(string query);
    }
}
