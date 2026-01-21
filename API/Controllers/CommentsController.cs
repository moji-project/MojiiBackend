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
    public async Task<ActionResult> CreateComment([FromBody] CommentDto comment)
    {
        await commentService.CreateComment(comment);
        return Ok();
    }
}