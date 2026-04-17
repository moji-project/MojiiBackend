using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Services;

namespace MojiiBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FeedbackController(FeedbackService feedbackService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> SendFeedback([FromBody] CreateFeedbackRequestDto createFeedbackRequestDto)
    {
        try
        {
            await feedbackService.SendFeedback(createFeedbackRequestDto);
            return Ok(new { message = "Feedback sent successfully." });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}

