using Clickly.Data.Dtos;
using Clickly.Data.Models;

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

        Task<GetNotificationDto> TogglePostLikeAsync(int postId, int userId);
        Task<GetNotificationDto> TogglePostFavoriteAsync(int postId, int userId);
        Task ToggleVisibilityPostAsync(int postId, int userId);
        Task ReportPostAsync(int postId, int userId);
    }
}
