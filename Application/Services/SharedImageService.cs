namespace MojiiBackend.Application.Services;

public class SharedImageService (IHttpContextAccessor _httpContextAccessor)
{
    public string BuildAbsoluteUrl(string relativePath)
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        if (request == null)
            return relativePath;

        return $"{request.Scheme}://{request.Host}{relativePath}";
    }
}