using Clickly.Data.Models;
using Microsoft.AspNetCore.Http;

namespace Clickly.ServiceContracts
{
    public interface IPostService
    {
        Task<List<Post>> GetAllPostsAsync(int loggedInUserId);
        Task<Post> GetPostByIdAsync(int postId);
        Task<List<Post>> GetAllFavoritedPostAsync(int loggedInUserId);
        Task<Post> CreatePostAsync(Post post);
        Task<Post> RemovePostAsync(int postId);

        Task AddPostCommentAsync(Comment comment);
        Task DeletePostCommentAsync(int commentId);

        Task TogglePostLikeAsync(int postId, int userId);
        Task TogglePostFavoriteAsync(int postId, int userId);
        Task ToggleVisibilityPostAsync(int postId, int userId);
        Task ReportPostAsync(int postId, int userId);
    }
}
