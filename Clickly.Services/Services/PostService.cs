using Clickly.Data;
using Clickly.Data.Dtos;
using Clickly.Data.Models;
using Clickly.ServiceContracts;
using Microsoft.EntityFrameworkCore;

namespace Clickly.Services.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly INotificationsService _notificationsService;

        public PostService(ApplicationDbContext dbContext, INotificationsService notificationsService)
        {
            _dbContext = dbContext;
            _notificationsService = notificationsService;
        }
        public async Task<List<Post>> GetAllPostsAsync(int loggedInUserId)
        {
            var allPosts = await _dbContext.Posts
                .Where(n => (!n.IsPrivate || n.UserId == loggedInUserId) && n.Reports.Count < 10 && !n.IsDeleted) // the post i created or the post that are public
                .Include(n => n.User)
                .Include(n => n.Like)
                .Include(n => n.Favorites)
                .Include(n => n.Comments).ThenInclude(n => n.User).OrderByDescending(n => n.DateCreated)
                .Include(n => n.Reports)
                .ToListAsync();

            return allPosts;

        }

        public async Task AddPostCommentAsync(Comment comment)
        {
            await _dbContext.Comments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();
            
        }

        public async Task<Post> CreatePostAsync(Post post)
        {

            
            await _dbContext.Posts.AddAsync(post);
            await _dbContext.SaveChangesAsync();

            return post;
        }

        public async Task DeletePostCommentAsync(int postId)
        {
            var commentDb = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == postId);
            if (commentDb != null)
            {
                _dbContext.Comments.Remove(commentDb);
                await _dbContext.SaveChangesAsync();
            }
        }


        public async Task<Post> RemovePostAsync(int postId)
        {
            var postDatabase = await _dbContext.Posts.FirstOrDefaultAsync(c => c.Id == postId);
            if (postDatabase != null)
            {
                postDatabase.IsDeleted = true;
                _dbContext.Posts.Update(postDatabase);
                await _dbContext.SaveChangesAsync();
            }
            return postDatabase;
        }

        public async Task ReportPostAsync(int postId, int userId)
        {
            var newReport = new Report()
            {
                UserId = userId,
                PostId = postId,
                DateCreated = DateTime.UtcNow,

            };

            await _dbContext.Reports.AddAsync(newReport);
            await _dbContext.SaveChangesAsync();

            var post = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if(post != null)
            {
                post.NumberOfReport += 1;
                _dbContext.Posts.Update(post);
                await _dbContext.SaveChangesAsync();
            }
            
        }

        public async Task<GetNotificationDto> TogglePostFavoriteAsync(int postId, int userId)
        {
            var responce = new GetNotificationDto()
            {
                Success = true,
                SendNotification = false

            };
            var favorite = await _dbContext.Favorites
                .Where(l => l.PostId == postId && l.UserId == userId)
                .FirstOrDefaultAsync();

            if (favorite != null)
            {
                _dbContext.Favorites.Remove(favorite);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                var newFavorite = new Favorite()
                {
                    PostId = postId,
                    UserId = userId,
                    DateCreated = DateTime.UtcNow
                };
                await _dbContext.Favorites.AddAsync(newFavorite);
                await _dbContext.SaveChangesAsync();

                responce.SendNotification = true;
            }
            return responce;
        }

        public async Task<GetNotificationDto> TogglePostLikeAsync(int postId, int userId)
        {
            var responce = new GetNotificationDto()
            {
                Success = true,
                SendNotification = false
                
            };
            var like = await _dbContext.Likes.Where(like => like.PostId == postId && like.UserId == userId).FirstOrDefaultAsync();
            if (like != null)
            {
                _dbContext.Likes.Remove(like);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                var newLike = new Like()
                {
                    PostId = postId,
                    UserId = userId,
                };
                await _dbContext.Likes.AddAsync(newLike);
                await _dbContext.SaveChangesAsync();

                responce.SendNotification = true;
            }
            return responce;
        }

        public async Task ToggleVisibilityPostAsync(int postId, int userId)
        {
            var post = await _dbContext.Posts.FirstOrDefaultAsync(l => l.Id == postId && l.UserId == userId);
            if (post != null)
            {
                post.IsPrivate = !post.IsPrivate;
                _dbContext.Posts.Update(post);
                await _dbContext.SaveChangesAsync();
            };
        }

        public async Task<List<Post>> GetAllFavoritedPostAsync(int loggedInUserId)
        {

            var allFavoritesPost = await _dbContext.Favorites
                .Include(f => f.Post.Reports)
                .Include(f => f.Post.User)
                .Include(f => f.Post.Comments)
                    .ThenInclude(c => c.User)
                .Include(f => f.Post.Like)
                .Include(f => f.Post.Favorites)
                .Where(f => f.UserId == loggedInUserId && !f.Post.IsDeleted && f.Post.Reports.Count < 5)
                .OrderByDescending(f => f.DateCreated)
                .Select(n => n.Post)
                .ToListAsync();

            return allFavoritesPost ;
        }

        public async Task<Post> GetPostByIdAsync(int postId)
        {
            var postDatabase = await _dbContext.Posts
                .Include(n => n.User)
                .Include(n => n.Like)
                .Include(n => n.Favorites)
                .Include(n => n.Comments)
                .Include(n => n.Reports)
                .FirstOrDefaultAsync(n => n.Id == postId);


            return postDatabase;
        }
    }
}
