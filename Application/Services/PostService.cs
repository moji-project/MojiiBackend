using Mapster;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Repositories;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.Application.Services;

public class PostService (PostRepository postRepository)
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
    
    public async Task DeletePost(int id)
    {
        await postRepository.Delete(id);
    }
}