namespace Dogstagram.Server.Features.Follows
{
    using Dogstagram.Server.Data;
    using Dogstagram.Server.Data.Models;
    using Dogstagram.Server.Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class FollowService : IFollowService
    {
        private readonly DogstagramDbContext context;

        public FollowService(DogstagramDbContext context)
        {
            this.context = context;
        }

        public async Task<Result> Follow(string userId, string followerId)
        {
            bool userAlreadyFollowed = await this.context
                .Follows
                .AnyAsync(f => f.UserId == userId && f.FollowerId == followerId);

            if (userAlreadyFollowed) 
                return "This user is already followed";

            bool publicProfile = await this.context
                .Profiles
                .Where(p => p.UserId == userId)
                .Select(p => !p.IsPrivate)
                .FirstOrDefaultAsync();

            Follow follow = new Follow
            {
                UserId = userId,
                FollowerId = followerId,
                IsApproved = publicProfile
            };

            this.context.Follows.Add(follow);

            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsFollower(string userId, string followerId)
        {
            return await this.context
                .Follows
                .AnyAsync(f => f.UserId == userId && f.FollowerId == followerId && f.IsApproved);
        }
    }
}
