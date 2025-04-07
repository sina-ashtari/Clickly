
using Clickly.Data;
using Clickly.Data.Helper;
using Clickly.Data.Models;
using Clickly.ServiceContracts;
using Microsoft.EntityFrameworkCore;

namespace Clickly.Services
{
    public class HashtagsService : IHashtagsService
    {
        private readonly ApplicationDbContext _dbContext;
        public HashtagsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task ProccessHashtagForNewPostAsync(string postContent)
        {
            var postHashtags = HashtagHelper.GetHashtags(postContent);
            if (postHashtags != null)
            {
                foreach (var hashtag in postHashtags)
                {
                    var hashtagDb = await _dbContext.Hashtags.FirstOrDefaultAsync(n => n.Name == hashtag);

                    if (hashtagDb != null)
                    {
                        hashtagDb.Count += 1;
                        hashtagDb.DateUpdated = DateTime.UtcNow;

                        _dbContext.Hashtags.Update(hashtagDb);
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        var newHashtag = new Hashtags { Name = hashtag, Count = 1, DateCreated = DateTime.UtcNow, DateUpdated = DateTime.UtcNow };
                        await _dbContext.Hashtags.AddAsync(newHashtag);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task ProccessHashtagForRemovePostAsync(string postContent)
        {
            var postHashtag = HashtagHelper.GetHashtags(postContent);
            foreach (var hashtag in postHashtag)
            {
                var hashtagDb = await _dbContext.Hashtags.FirstOrDefaultAsync(n => n.Name == hashtag);
                if (hashtagDb != null)
                {
                    hashtagDb.Count -= 1;
                    hashtagDb.DateUpdated = DateTime.UtcNow;

                    _dbContext.Hashtags.Update(hashtagDb);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
