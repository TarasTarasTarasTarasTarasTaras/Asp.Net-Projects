namespace Dogstagram.Server.Features.Search
{
    using Dogstagram.Server.Features.Search.Models;
    using Dogstagram.Server.Infrastructure.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class SearchController : ApiController
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        [Route(nameof(Profiles))]
        public async Task<IEnumerable<ProfileSearchResponseModel>> Profiles(string query)
        {
            return await this._searchService.Profiles(query);
        }
    }
}
