public interface IImageUpload
{
    Task<string?> UploadFile(IFormFile file, string folderType, CancellationToken C);
  
}

public class ImageUpload(IWebHostEnvironment host) : IImageUpload
{
    public async Task<string?> UploadFile(IFormFile file, string folderType, CancellationToken C)
    {
        if (file == null) return null;

       
        string folder = $"images/{folderType}";

       
        string directoryPath = Path.Combine(host.WebRootPath, folder);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

       
        string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        string fileName = $"{folder}/{uniqueFileName}";
        string uploadPath = Path.Combine(host.WebRootPath, fileName);

        using var stream = File.Create(uploadPath);

        await file.CopyToAsync(stream, C);

       
        return $"/{fileName}"; 
    }
}