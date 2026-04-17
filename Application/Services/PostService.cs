using Mapster;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Repositories;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.Application.Services;

public class PostService (PostRepository postRepository, ICurrentUserService currentUserService)
{
    public async Task<List<PostDto>> GetAllPosts()
    {
        var posts = await postRepository.GetAll();
        return posts.Select(MapPostToDto).ToList();
    }

    public async Task<List<PostDto>> GetMostRecentWithSkip(int skip)
    {
        var posts = await  postRepository.GetMostRecentWithSkip(skip);
        return posts.Select(MapPostToDto).ToList();
    }

    public async Task<List<PostDto>> GetAllPostsByUserId(int userId)
    {
        var posts = await postRepository.GetPostsByUserId(userId);
        return posts.Select(MapPostToDto).ToList();
    }
    
    public async Task<PostDto> GetPostById(int postId)
    {
        var post = await postRepository.GetById(postId);
        if (post == null)
            throw new ArgumentException("Post not found.");

        return MapPostToDto(post);
    }

    public async Task<PostDto> CreatePost(PostDto postDto)
    {
        var normalizedImageUrls = NormalizeAndValidateImageUrls(postDto.ImageUrls, postDto.ImageUrl);
        var normalizedContent = postDto.Content?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(normalizedContent))
            throw new ArgumentException("Post content cannot be empty.");

        Post post = new()
        {
            Content = normalizedContent,
            ImageUrls = normalizedImageUrls,
            UserId = currentUserService.GetUserId(),
            User = null!
        };

        await postRepository.Create(post);
        var createdPost = await postRepository.GetById(post.Id);
        if (createdPost == null)
            throw new ArgumentException("Post not found.");

        return MapPostToDto(createdPost);
    }

    public async Task<PostDto> UpdatePost(PostDto postDto)
    {
        var existingPost = await postRepository.GetByIdForUpdate(postDto.Id);
        if (existingPost == null)
            throw new ArgumentException("Post not found.");

        var normalizedImageUrls = NormalizeAndValidateImageUrls(postDto.ImageUrls, postDto.ImageUrl);
        var normalizedContent = postDto.Content?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(normalizedContent))
            throw new ArgumentException("Post content cannot be empty.");

        existingPost.Content = normalizedContent;
        existingPost.ImageUrls = normalizedImageUrls;

        await postRepository.Update(existingPost);
        var updatedPost = await postRepository.GetById(existingPost.Id);
        if (updatedPost == null)
            throw new ArgumentException("Post not found.");

        return MapPostToDto(updatedPost);
    }
    
    public async Task DeletePost(int id)
    {
        await postRepository.Delete(id);
    }

    public async Task<PostDto> ToggleLike(int postId)
    {
        var userId = currentUserService.GetUserId();

        var post = await postRepository.GetById(postId);
        if (post == null)
            throw new Exception("Post not found");

        var hasLiked = post.HavingLikedUsers.Any(u => u.Id == userId);

        if (hasLiked)
            await postRepository.RemoveLike(postId, userId);
        else
            await postRepository.AddLike(postId, userId);

        var updatedPost = await postRepository.GetById(postId);
        if (updatedPost == null)
            throw new ArgumentException("Post not found.");

        return MapPostToDto(updatedPost);
    }

    private static PostDto MapPostToDto(Post post)
    {
        var postDto = post.Adapt<PostDto>();
        postDto.ImageUrls = (post.ImageUrls ?? [])
            .Where(IsSupportedImageUrl)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(5)
            .ToArray();

        postDto.ImageUrl = postDto.ImageUrls.FirstOrDefault();
        postDto.LikesCount = post.HavingLikedUsers.Count;
        postDto.CommentsCount = post.Comments.Count;
        return postDto;
    }

    private static string[] NormalizeAndValidateImageUrls(IEnumerable<string>? imageUrls, string? legacyImageUrl)
    {
        var normalizedUrls = new List<string>();

        if (imageUrls is not null)
            normalizedUrls.AddRange(imageUrls);

        if (!string.IsNullOrWhiteSpace(legacyImageUrl))
            normalizedUrls.Add(legacyImageUrl);

        normalizedUrls = normalizedUrls
            .Where(url => !string.IsNullOrWhiteSpace(url))
            .Select(url => url.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (normalizedUrls.Count > 5)
            throw new ArgumentException("A post can contain at most 5 images.");

        if (normalizedUrls.Any(url => url.Length > 500))
            throw new ArgumentException("Each image URL must be at most 500 characters.");

        if (normalizedUrls.Any(url => !IsSupportedImageUrl(url)))
        {
            throw new ArgumentException(
                "Each image must be an absolute http(s) URL. Local file paths like C:\\\\... are not supported.");
        }

        return normalizedUrls.ToArray();
    }

    private static bool IsSupportedImageUrl(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            return false;

        return (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps) &&
               !string.IsNullOrWhiteSpace(uri.Host);
    }
}
