using Mapster;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Repositories;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.Application.Services;

public class NotificationService(NotificationRepository notificationRepository, ICurrentUserService currentUserService)
{
    public async Task<List<NotificationDto>> GetForConnectedUser()
    {
        var userId = currentUserService.GetUserId();
        var notifications = await notificationRepository.GetForUser(userId);
        return notifications.Adapt<List<NotificationDto>>();
    }

    public async Task<NotificationDto> CreateNotification(NotificationDto notificationDto)
    {
        var notification = notificationDto.Adapt<Notification>();
        await notificationRepository.Create(notification);
        return notification.Adapt<NotificationDto>();
    }

    public async Task<NotificationDto> UpdateNotification(NotificationDto notificationDto)
    {
        var notification = notificationDto.Adapt<Notification>();
        await notificationRepository.Update(notification);
        return notification.Adapt<NotificationDto>();
    }

    public async Task DeleteNotification(int id)
    {
        await notificationRepository.Delete(id);
    }
}
