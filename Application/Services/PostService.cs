using Mapster;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Repositories;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.Application.Services;

public class PostService (PostRepository postRepository, ICurrentUserService currentUserService)
{
    public async Task CreatePost(PostDto postDto)
    {
        Post post = postDto.Adapt<Post>();
        await postRepository.Create(post);
    }
    
    public async Task<List<PostDto>> GetAllPosts()
    {
        var posts = await postRepository.GetAll();
        return posts.Adapt<List<PostDto>>();
    }

    public async Task UpdatePost(PostDto postDto)
    {
        Post post = postDto.Adapt<Post>();
        await postRepository.Update(post);
    }
    
    public async Task DeletePost(int id)
    {
        await postRepository.Delete(id);
    }

    public async Task ToggleLike(int postId)
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
    }
}