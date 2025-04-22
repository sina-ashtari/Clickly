using Clickly.Data.Models;

namespace Clickly.ServiceContracts
{
    public interface IAdminService
    {
        Task<List<Post>> GetReportedPostAsync();
        Task ApproveReportAsync(int postId);
        Task RejectReportAsync(int postId);
    }
}
