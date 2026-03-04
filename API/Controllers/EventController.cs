using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Services;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EventController (EventService eventService) : ControllerBase
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

    [HttpGet("organization/{organizationId:int}")]
    public async Task<ActionResult<List<EventDto>>> GetAllByOrganization(int organizationId)
    {
        var events = await eventService.GetAllByOrganization(organizationId);
        return Ok(events);
    }

    [HttpPost]
    public async Task<ActionResult<EventDto>> CreateEvent([FromBody] EventDto eventDto)
    {
        var createdEvent = await eventService.CreateEvent(eventDto);
        return CreatedAtAction(nameof(GetEventById), new { eventId = createdEvent.Id }, createdEvent);
    }

    [HttpPut("{eventId:int}")]
    public async Task<ActionResult<EventDto>> UpdateEvent(int eventId, [FromBody] EventDto eventDto)
    {
        var updatedEvent = await eventService.UpdateEvent(eventId, eventDto);
        return Ok(updatedEvent);
    }

    [HttpDelete("{eventId:int}")]
    public async Task<ActionResult> DeleteEvent(int eventId)
    {
        await eventService.DeleteEvent(eventId);
        return Ok();
    }

    [HttpPost("ToggleInterested/{eventId:int}")]
    public async Task<ActionResult> ToggleInterested(int eventId)
    {
        await eventService.ToggleInterested(eventId);
        return Ok();
    }
}