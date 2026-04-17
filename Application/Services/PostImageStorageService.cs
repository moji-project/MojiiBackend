using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace MojiiBackend.Application.Services;

public class PostImageStorageService(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
{
    private const int MaxFilesPerRequest = 5;
    private const long MaxFileSizeBytes = 8 * 1024 * 1024; // 8 MB per file

    private static readonly HashSet<string> AllowedExtensions =
    [
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    ];

    public async Task<List<string>> UploadPostImages(IReadOnlyCollection<IFormFile> files, CancellationToken cancellationToken = default)
    {
        if (files.Count == 0)
            throw new ArgumentException("You must upload at least one image.");

        if (files.Count > MaxFilesPerRequest)
            throw new ArgumentException("You can upload at most 5 images per request.");

        var webRoot = environment.WebRootPath ?? Path.Combine(environment.ContentRootPath, "wwwroot");
        var uploadsFolder = Path.Combine(webRoot, "uploads", "posts");
        Directory.CreateDirectory(uploadsFolder);

        var uploadedUrls = new List<string>();

        foreach (var file in files)
        {
            if (file.Length == 0)
                throw new ArgumentException("One of the uploaded files is empty.");

            if (file.Length > MaxFileSizeBytes)
                throw new ArgumentException("Each image must be 8 MB or less.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                throw new ArgumentException("Only .jpg, .jpeg, .png and .webp files are allowed.");

            var fileName = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            await using (var stream = File.Create(filePath))
            {
                await file.CopyToAsync(stream, cancellationToken);
            }

            uploadedUrls.Add(BuildAbsoluteUrl($"/uploads/posts/{fileName}"));
        }

        return uploadedUrls;
    }

    private string BuildAbsoluteUrl(string relativePath)
    {
        var request = httpContextAccessor.HttpContext?.Request;
        if (request == null)
            return relativePath;

        return $"{request.Scheme}://{request.Host}{relativePath}";
    }
}
