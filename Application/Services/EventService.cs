using Mapster;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Repositories;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.Application.Services;

public class EventService (EventRepository eventRepository, ICurrentUserService currentUserService, ImageService imageService)
{
    public async Task<List<EventDto>> GetAllByOrganization(int organizationId)
    {
        var events = await eventRepository.GetAllByOrganization(organizationId);
        return events.Adapt<List<EventDto>>();
    }

    public async Task<List<EventDto>> GetAllEvents()
    {
        var events = await eventRepository.GetAll();
        return events.Adapt<List<EventDto>>();
    }

    public async Task<EventDto?> GetEventById(int eventId)
    {
        var eventEntity = await eventRepository.GetById(eventId);
        return eventEntity?.Adapt<EventDto>();
    }
    
    public async Task<EventDto> CreateEvent(EventDto eventDto)
    {
        var userId = currentUserService.GetUserId();

        var eventEntity = eventDto.Adapt<Event>();
        eventEntity.CreatorUserId = userId;

        await eventRepository.Create(eventEntity);
        return eventEntity.Adapt<EventDto>();
    }

    public async Task<EventDto> UpdateEvent(int eventId, EventDto eventDto)
    {
        var existingEvent = await eventRepository.GetById(eventId);
        if (existingEvent == null)
            throw new Exception("Event not found");

        existingEvent.Name = eventDto.Name;
        existingEvent.StartDate = eventDto.StartDate;
        existingEvent.ImageUrl = eventDto.ImageUrl;
        existingEvent.IsPublished = eventDto.IsPublished;

        await eventRepository.Update(existingEvent);
        return existingEvent.Adapt<EventDto>();
    }

    public async Task DeleteEvent(int eventId)
    {
        await eventRepository.Delete(eventId);
    }

    public async Task ToggleInterested(int eventId)
    {
        var userId = currentUserService.GetUserId();

        var eventEntity = await eventRepository.GetById(eventId);
        if (eventEntity == null)
            throw new Exception("Event not found");

        var isInterested = eventEntity.InterestedUsers.Any(u => u.Id == userId);

        if (isInterested)
            await eventRepository.RemoveInterested(eventEntity, userId);
        else
            await eventRepository.AddInterested(eventEntity, userId);
    }

    public async Task<string> UploadImageAsync(int eventId, IFormFile file)
    {
        var eventEntity = await eventRepository.GetById(eventId);
        if (eventEntity == null)
            throw new Exception("Event not found");

        var imageUrl = await imageService.SaveImageAsync(file);
        eventEntity.ImageUrl = imageUrl;
        
        await eventRepository.Update(eventEntity);
        
        return imageUrl;
    }
}