using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Services;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PostsController (PostService postService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<PostDto>>> GetAllPosts()
    {
        var posts = await postService.GetAllPosts();
        return Ok(posts);
    }
    
    [HttpPost]
    public async Task<ActionResult> CreatePost([FromBody] PostDto post)
    {
        await postService.CreatePost(post);
        return Ok();
    }

    [HttpDelete("{postId:int}")]
    public async Task<ActionResult> DeletePost(int postId)
    {
        await postService.DeletePost(postId);
        return Ok();
    }

    [HttpPost("ToggleLike")]
    public async Task<ActionResult> ToggleLike([FromBody] int postId)
    {
        await postService.ToggleLike(postId);
        return Ok();
    }
}
//todo dès que un admin d orga se connecte, comment l'app le détecte et lui renvoie son organozaation