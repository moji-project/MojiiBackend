using Mapster;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Repositories;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.Application.Services;

public class EventService (EventRepository eventRepository, CommentRepository commentRepository, ICurrentUserService currentUserService)
{
    public async Task<List<EventDto>> GetAllByOrganization(int organizationId)
    {
        var currentUserId = currentUserService.GetUserId();
        var events = await eventRepository.GetAllByOrganization(organizationId);
        var commentsCountByEventId = await GetCommentsCountByEventIds(events.Select(e => e.Id).ToList());

        return events
            .Select(e => MapEventToDto(e, currentUserId, commentsCountByEventId))
            .ToList();
    }

    public async Task<List<EventDto>> GetAllEvents()
    {
        var currentUserId = currentUserService.GetUserId();
        var events = await eventRepository.GetAll();
        var commentsCountByEventId = await GetCommentsCountByEventIds(events.Select(e => e.Id).ToList());

        return events
            .Select(e => MapEventToDto(e, currentUserId, commentsCountByEventId))
            .ToList();
    }

    public async Task<EventDto?> GetEventById(int eventId)
    {
        var currentUserId = currentUserService.GetUserId();
        var eventEntity = await eventRepository.GetById(eventId);
        if (eventEntity == null)
            return null;

        var commentsCountByEventId = await GetCommentsCountByEventIds([eventEntity.Id]);
        return MapEventToDto(eventEntity, currentUserId, commentsCountByEventId);
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
        existingEvent.Description = eventDto.Description;
        existingEvent.Location = eventDto.Location;
        existingEvent.Address = eventDto.Address;
        existingEvent.DateLabel = eventDto.DateLabel;
        existingEvent.MonthLabel = eventDto.MonthLabel;
        existingEvent.DayLabel = eventDto.DayLabel;
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

    public async Task ToggleLike(int eventId)
    {
        await ToggleInterested(eventId);
    }

    public async Task<List<CommentDto>> GetEventComments(int eventId)
    {
        var eventExists = await eventRepository.Exists(eventId);
        if (!eventExists)
            throw new ArgumentException("Event not found.");

        var comments = await commentRepository.GetByEventId(eventId);
        return comments.Adapt<List<CommentDto>>();
    }

    public async Task<CommentDto> CreateEventComment(int eventId, CreateEventCommentDto createEventCommentDto)
    {
        var eventExists = await eventRepository.Exists(eventId);
        if (!eventExists)
            throw new ArgumentException("Event not found.");

        if (string.IsNullOrWhiteSpace(createEventCommentDto.Content))
            throw new ArgumentException("Comment content cannot be empty.");

        var normalizedContent = createEventCommentDto.Content.Trim();
        if (normalizedContent.Length > 500)
            throw new ArgumentException("Comment content cannot exceed 500 characters.");

        var comment = new Comment
        {
            Content = normalizedContent,
            EventId = eventId,
            UserId = currentUserService.GetUserId(),
            User = null!
        };

        await commentRepository.Create(comment);
        var createdComment = await commentRepository.GetByIdWithRelations(comment.Id);
        return (createdComment ?? comment).Adapt<CommentDto>();
    }

    public async Task<EventShareDto> GetEventSharePayload(int eventId, string baseUrl)
    {
        var eventEntity = await eventRepository.GetById(eventId);
        if (eventEntity == null)
            throw new ArgumentException("Event not found.");

        var normalizedBaseUrl = baseUrl.TrimEnd('/');
        var shareUrl = $"{normalizedBaseUrl}/api/Event/{eventEntity.Id}";

        return new EventShareDto
        {
            EventId = eventEntity.Id,
            EventName = eventEntity.Name,
            ShareUrl = shareUrl,
            ShareText = $"Rejoins-moi a l'evenement \"{eventEntity.Name}\" : {shareUrl}"
        };
    }

    private async Task<Dictionary<int, int>> GetCommentsCountByEventIds(List<int> eventIds)
    {
        return await commentRepository.GetCommentsCountByEventIds(eventIds);
    }

    private static EventDto MapEventToDto(Event eventEntity, int currentUserId, IReadOnlyDictionary<int, int> commentsCountByEventId)
    {
        var eventDto = eventEntity.Adapt<EventDto>();
        eventDto.IsInterestedByCurrentUser = eventEntity.InterestedUsers.Any(u => u.Id == currentUserId);
        var dynamicInterestedCount = eventEntity.InterestedUsers.Count;
        eventDto.InterestedCount = dynamicInterestedCount;
        eventDto.IsLikedByCurrentUser = eventDto.IsInterestedByCurrentUser;
        eventDto.LikesCount = dynamicInterestedCount;
        eventDto.CommentsCount = commentsCountByEventId.TryGetValue(eventEntity.Id, out var count) ? count : 0;
        return eventDto;
    }
}
