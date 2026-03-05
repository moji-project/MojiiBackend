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

    [HttpGet("GetMostRecentWithSkip/{skip:int}")]
    public async Task<ActionResult<List<PostDto>>> GetMostRecentWithSkip(int skip)
    {
        var posts = await postService.GetMostRecentWithSkip(skip);
        return  Ok(posts);
    }

    [HttpGet("GetByUserId/{userId:int}")]
    public async Task<ActionResult<List<PostDto>>> GetByUserId(int userId)
    {
        var posts = await postService.GetAllPostsByUserId(userId);
        return  Ok(posts);
    }
    
    [HttpPost]
    public async Task<ActionResult> CreatePost([FromBody] PostDto postDto)
    {
        await postService.CreatePost(postDto);
        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> UpdatePost([FromBody] PostDto postDto)
    {
        await postService.UpdatePost(postDto);
        return Ok();
    }

    [HttpDelete("{postId:int}")]
    public async Task<ActionResult> DeletePost(int postId)
    {
        await postService.DeletePost(postId);
        return Ok();
    }

    [HttpPost("ToggleLike/{postId}")]
    public async Task<ActionResult> ToggleLike(int postId)
    {
        await postService.ToggleLike(postId);
        return Ok();
    }
}
//todo dès que un admin d orga se connecte, comment l'app le détecte et lui renvoie son organozaation