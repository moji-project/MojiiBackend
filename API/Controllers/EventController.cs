using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Services;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EventController (EventService eventService, RealtimeService realtimeService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<EventDto>>> GetAllEvents()
    {
        var events = await eventService.GetAllEvents();
        return Ok(events);
    }

    [HttpGet("{eventId:int}")]
    public async Task<ActionResult<EventDto>> GetEventById(int eventId)
    {
        var eventDto = await eventService.GetEventById(eventId);
        if (eventDto == null)
            return NotFound();

        return Ok(eventDto);
    }

    [HttpGet("GetAllByOrganization/{organizationId:int}")]
    public async Task<ActionResult<List<EventDto>>> GetAllByOrganization(int organizationId)
    {
        var events = await eventService.GetAllByOrganization(organizationId);
        return Ok(events);
    }

    [HttpPost]
    public async Task<ActionResult<EventDto>> CreateEvent([FromBody] EventDto eventDto)
    {
        var createdEvent = await eventService.CreateEvent(eventDto);
        await realtimeService.BroadcastEntityChanged("Event", "Created", createdEvent);
        return CreatedAtAction(nameof(GetEventById), new { eventId = createdEvent.Id }, createdEvent);
    }

    [HttpPut("{eventId:int}")]
    public async Task<ActionResult<EventDto>> UpdateEvent(int eventId, [FromBody] EventDto eventDto)
    {
        var updatedEvent = await eventService.UpdateEvent(eventId, eventDto);
        await realtimeService.BroadcastEntityChanged("Event", "Updated", updatedEvent);
        return Ok(updatedEvent);
    }

    [HttpDelete("{eventId:int}")]
    public async Task<ActionResult> DeleteEvent(int eventId)
    {
        await eventService.DeleteEvent(eventId);
        await realtimeService.BroadcastEntityChanged("Event", "Deleted", new { id = eventId });
        return Ok();
    }

    [HttpPost("ToggleInterested/{eventId:int}")]
    public async Task<ActionResult> ToggleInterested(int eventId)
    {
        await eventService.ToggleInterested(eventId);
        var updatedEvent = await eventService.GetEventById(eventId);
        if (updatedEvent != null)
            await realtimeService.BroadcastEntityChanged("Event", "Updated", updatedEvent);
        return Ok();
    }

    [HttpPost("ToggleLike/{eventId:int}")]
    public async Task<ActionResult> ToggleLike(int eventId)
    {
        await eventService.ToggleLike(eventId);
        var updatedEvent = await eventService.GetEventById(eventId);
        if (updatedEvent != null)
            await realtimeService.BroadcastEntityChanged("Event", "Updated", updatedEvent);
        return Ok();
    }

    [HttpGet("{eventId:int}/Comments")]
    public async Task<ActionResult<List<CommentDto>>> GetComments(int eventId)
    {
        try
        {
            var comments = await eventService.GetEventComments(eventId);
            return Ok(comments);
        }
        catch (ArgumentException ex)
        {
            if (ex.Message == "Event not found.")
                return NotFound(new { error = ex.Message });

            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("{eventId:int}/Comments")]
    public async Task<ActionResult<CommentDto>> CreateComment(int eventId, [FromBody] CreateEventCommentDto createEventCommentDto)
    {
        try
        {
            var comment = await eventService.CreateEventComment(eventId, createEventCommentDto);
            await realtimeService.BroadcastEntityChanged("Comment", "Created", comment);
            var updatedEvent = await eventService.GetEventById(eventId);
            if (updatedEvent != null)
                await realtimeService.BroadcastEntityChanged("Event", "Updated", updatedEvent);
            return Ok(comment);
        }
        catch (ArgumentException ex)
        {
            if (ex.Message == "Event not found.")
                return NotFound(new { error = ex.Message });

            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{eventId:int}/Share")]
    public async Task<ActionResult<EventShareDto>> GetSharePayload(int eventId)
    {
        try
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var payload = await eventService.GetEventSharePayload(eventId, baseUrl);
            return Ok(payload);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpPost("{eventId:int}/Share")]
    public async Task<ActionResult<EventShareDto>> Share(int eventId)
    {
        try
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var payload = await eventService.GetEventSharePayload(eventId, baseUrl);
            return Ok(payload);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}
