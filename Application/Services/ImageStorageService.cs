using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace MojiiBackend.Application.Services;

public class ImageStorageService(IWebHostEnvironment environment, SharedImageService sharedImageService)
{
    private const long MaxFileSizeBytes = 8 * 1024 * 1024; // 8 MB

    private static readonly HashSet<string> AllowedExtensions =
    [
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    ];

    public async Task<string> UploadImage(IFormFile file, string directoryToUploadTo, CancellationToken cancellationToken = default)
    {
        if (file.Length == 0)
            throw new ArgumentException("The uploaded file is empty.");

        if (file.Length > MaxFileSizeBytes)
            throw new ArgumentException("Image must be 8 MB or less.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
            throw new ArgumentException("Only .jpg, .jpeg, .png and .webp files are allowed.");

        var webRoot = environment.WebRootPath ?? Path.Combine(environment.ContentRootPath, "wwwroot");
        var uploadsFolder = Path.Combine(webRoot, "uploads", directoryToUploadTo);
        Directory.CreateDirectory(uploadsFolder);

        var fileName = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}{extension}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        await using (var stream = File.Create(filePath))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        return sharedImageService.BuildAbsoluteUrl($"/uploads/{directoryToUploadTo}/{fileName}");
    }
}
