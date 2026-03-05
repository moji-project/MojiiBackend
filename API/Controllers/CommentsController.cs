using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Services;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommentsController (CommentService commentService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> CreateComment([FromBody] CommentDto commentDto)
    {
        await commentService.CreateComment(commentDto);
        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> UpdateComment([FromBody] CommentDto commentDto)
    {
        await commentService.UpdateComment(commentDto);
        return Ok();
    }

    [HttpDelete("{commentId:int}")]
    public async Task<ActionResult> DeleteComment(int commentId)
    {
        await commentService.DeleteComment(commentId);
        return Ok();
    }
}