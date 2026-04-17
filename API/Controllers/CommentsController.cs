using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Services;

namespace MojiiBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommentsController (
    CommentService commentService,
    PostService postService,
    EventService eventService,
    RealtimeService realtimeService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CommentDto>>> GetComments([FromQuery] int? postId, [FromQuery] int? eventId)
    {
        if (postId.HasValue == eventId.HasValue)
            return BadRequest("You must provide exactly one query parameter: postId or eventId.");

        if (postId.HasValue)
        {
            var postComments = await commentService.GetByPostId(postId.Value);
            return Ok(postComments);
        }

        var eventComments = await commentService.GetByEventId(eventId!.Value);
        return Ok(eventComments);
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> CreateComment([FromBody] CommentDto commentDto)
    {
        try
        {
            var createdComment = await commentService.CreateComment(commentDto);
            await realtimeService.BroadcastEntityChanged("Comment", "Created", createdComment);

            if (createdComment.PostId.HasValue)
            {
                var updatedPost = await postService.GetPostById(createdComment.PostId.Value);
                await realtimeService.BroadcastEntityChanged("Post", "Updated", updatedPost);
            }

            if (createdComment.EventId.HasValue)
            {
                var updatedEvent = await eventService.GetEventById(createdComment.EventId.Value);
                if (updatedEvent != null)
                    await realtimeService.BroadcastEntityChanged("Event", "Updated", updatedEvent);
            }

            return Ok(createdComment);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut]
    public async Task<ActionResult<CommentDto>> UpdateComment([FromBody] CommentDto commentDto)
    {
        try
        {
            var updatedComment = await commentService.UpdateComment(commentDto);
            await realtimeService.BroadcastEntityChanged("Comment", "Updated", updatedComment);

            if (updatedComment.PostId.HasValue)
            {
                var updatedPost = await postService.GetPostById(updatedComment.PostId.Value);
                await realtimeService.BroadcastEntityChanged("Post", "Updated", updatedPost);
            }

            if (updatedComment.EventId.HasValue)
            {
                var updatedEvent = await eventService.GetEventById(updatedComment.EventId.Value);
                if (updatedEvent != null)
                    await realtimeService.BroadcastEntityChanged("Event", "Updated", updatedEvent);
            }

            return Ok(updatedComment);
        }
        catch (ArgumentException ex)
        {
            if (ex.Message == "Comment not found.")
                return NotFound(new { error = ex.Message });

            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{commentId:int}")]
    public async Task<ActionResult> DeleteComment(int commentId)
    {
        try
        {
            var deletedComment = await commentService.DeleteComment(commentId);
            await realtimeService.BroadcastEntityChanged("Comment", "Deleted",
                new { id = commentId, deletedComment.PostId, deletedComment.EventId });

            if (deletedComment.PostId.HasValue)
            {
                var updatedPost = await postService.GetPostById(deletedComment.PostId.Value);
                await realtimeService.BroadcastEntityChanged("Post", "Updated", updatedPost);
            }

            if (deletedComment.EventId.HasValue)
            {
                var updatedEvent = await eventService.GetEventById(deletedComment.EventId.Value);
                if (updatedEvent != null)
                    await realtimeService.BroadcastEntityChanged("Event", "Updated", updatedEvent);
            }

            return Ok();
        }
        catch (ArgumentException ex)
        {
            if (ex.Message == "Comment not found.")
                return NotFound(new { error = ex.Message });

            return BadRequest(new { error = ex.Message });
        }
    }
}
