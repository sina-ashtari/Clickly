using Clickly.Data.Helper.Enums;
using Clickly.ServiceContracts;
using Microsoft.AspNetCore.Http;

namespace Clickly.Services.Services
{
    public class FilesService : IFilesService
    {
        public async Task<string> UploadImageAsync(IFormFile file, ImageFileType fileType)
        {
            string filePathUpload = fileType switch
            {
                ImageFileType.PostImage => Path.Combine("images", "posts"),
                ImageFileType.CoverImage => Path.Combine("images", "covers"),
                ImageFileType.ProfileImage => Path.Combine("images", "profilePictures"),
                ImageFileType.StoryImage => Path.Combine("images", "stories"),
                _ => throw new ArgumentException("Invalid file type")

            };

            if (file != null && file.Length > 0)
            {
                string rootFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                // validating if the provided image type is correct then saving it in wwwroot folder
                if (file.ContentType.Contains("image"))
                {
                    // first we trying to create root folder in that case we dont have it
                    string rootFolderPathImage = Path.Combine(rootFolderPath, filePathUpload);
                    Directory.CreateDirectory(rootFolderPathImage);

                    // then creating file name with defined Guid for each of them
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(rootFolderPathImage, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);

                        return $"/{filePathUpload}/{fileName}";
                    }
                }
            }
            return "";
        }
    }
}
