using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Services;

namespace MojiiBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PostsController(PostService postService, PostImageStorageService postImageStorageService, RealtimeService realtimeService) : ControllerBase
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
        return Ok(posts);
    }

    [HttpGet("GetByUserId/{userId:int}")]
    public async Task<ActionResult<List<PostDto>>> GetByUserId(int userId)
    {
        var posts = await postService.GetAllPostsByUserId(userId);
        return Ok(posts);
    }

    [HttpPost]
    public async Task<ActionResult> CreatePost([FromBody] PostDto postDto)
    {
        try
        {
            var createdPost = await postService.CreatePost(postDto);
            await realtimeService.BroadcastEntityChanged("Post", "Created", createdPost);
            return Ok(createdPost);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("UploadImages")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(40 * 1024 * 1024)]
    public async Task<ActionResult> UploadImages([FromForm] List<IFormFile> files, CancellationToken cancellationToken)
    {
        try
        {
            var imageUrls = await postImageStorageService.UploadPostImages(files, cancellationToken);
            return Ok(new { imageUrls });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut]
    public async Task<ActionResult> UpdatePost([FromBody] PostDto postDto)
    {
        try
        {
            var updatedPost = await postService.UpdatePost(postDto);
            await realtimeService.BroadcastEntityChanged("Post", "Updated", updatedPost);
            return Ok(updatedPost);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{postId:int}")]
    public async Task<ActionResult> DeletePost(int postId)
    {
        await postService.DeletePost(postId);
        await realtimeService.BroadcastEntityChanged("Post", "Deleted", new { id = postId });
        return Ok();
    }

    [HttpPost("ToggleLike/{postId}")]
    public async Task<ActionResult> ToggleLike(int postId)
    {
        var updatedPost = await postService.ToggleLike(postId);
        await realtimeService.BroadcastEntityChanged("Post", "Updated", updatedPost);
        return Ok(updatedPost);
    }
}
