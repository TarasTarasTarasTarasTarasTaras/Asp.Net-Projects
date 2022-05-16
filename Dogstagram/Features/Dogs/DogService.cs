namespace Dogstagram.Server.Features.Dogs
{
    using Dogstagram.Server.Data;
    using Dogstagram.Server.Data.Models;
    using Dogstagram.Server.Features.Dogs.Models;
    using Dogstagram.Server.Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DogService : IDogService
    {
        private readonly DogstagramDbContext context;

        public DogService(DogstagramDbContext context)
        {
            this.context = context;
        }

        public async Task<int> Create(string imageUrl, string description, string userId)
        {
            var dog = new Dog
            {
                ImageUrl = imageUrl,
                Description = description,
                UserId = userId
            };

            context.Add(dog);
            await context.SaveChangesAsync();

            return dog.Id;
        }


        public async Task<IEnumerable<DogListingResponseModel>> ByUser(string userId, string userName)
            => await this.context
                .Dogs
                .Where(d => d.UserId == userId)
                .OrderByDescending(d => d.CreatedOn)
                .Select(d => new DogListingResponseModel
                {
                    Id = d.Id,
                    ImageUrl = d.ImageUrl,
                    UserName = userName
                })
                .ToListAsync();


        public async Task<DogDetailsResponseModel> ByUserDetails(int id)
            => await this.context
            .Dogs
            .Where(d => d.Id == id).Select(d => new DogDetailsResponseModel
            {
                Id = d.Id,
                ImageUrl = d.ImageUrl,
                Description = d.Description,
                UserId = d.UserId,
                UserName = d.User.UserName
            })
            .FirstOrDefaultAsync();


        public async Task<Result> Update(int id, string description, string userId)
        {
            var dog = await this.context
                .Dogs
                .Where(d => d.Id == id && d.UserId == userId)
                .FirstOrDefaultAsync();

            if (dog is null) return "This user cannot edit this dog";

            dog.Description = description;
            await this.context.SaveChangesAsync();

            return true;
        }


        public async Task<Result> Delete(int id, string userId)
        {
            var dog = await this.context.Dogs.FirstOrDefaultAsync(d => d.Id == id && d.UserId == userId);
            if (dog is null) return "This user cannot delete this dog";

            this.context.Dogs.Remove(dog);
            await this.context.SaveChangesAsync();

            return true;
        }
    }
}
