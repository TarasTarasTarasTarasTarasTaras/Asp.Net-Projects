namespace Dogstagram.Server.Features.Dogs
{
    using Dogstagram.Server.Data.Models;
    using Dogstagram.Server.Features.Dogs.Models;
    using Dogstagram.Server.Infrastructure.Services;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDogService
    {
        Task<int> Create(string imageUrl, string description, string userId);

        Task<IEnumerable<DogListingResponseModel>> ByUser(string userId, string userName);

        Task<DogDetailsResponseModel> ByUserDetails(int id);

        Task<Result> Update(int id, string description, string userId);

        Task<Result> Delete(int id, string userId);
    }
}
