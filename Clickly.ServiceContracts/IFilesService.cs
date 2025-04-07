using Clickly.Data.Helper.Enums;
using Microsoft.AspNetCore.Http;

namespace Clickly.ServiceContracts
{
    public interface IFilesService
    {
        Task<string> UploadImageAsync(IFormFile image, ImageFileType fileType);
    }
}
