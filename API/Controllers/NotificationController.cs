using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Services;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController(NotificationService notificationService, RealtimeService realtimeService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<NotificationDto>>> GetForConnectedUser()
    {
        var notifications = await notificationService.GetForConnectedUser();
        return Ok(notifications);
    }

    [HttpPost]
    public async Task<ActionResult<NotificationDto>> CreateNotification([FromBody] NotificationDto notificationDto)
    {
        var notification = await notificationService.CreateNotification(notificationDto);
        await realtimeService.BroadcastEntityChanged("Notification", "Created", notification);
        return Ok(notification);
    }

    [HttpPut]
    public async Task<ActionResult<NotificationDto>> UpdateNotification([FromBody] NotificationDto notificationDto)
    {
        var notification = await notificationService.UpdateNotification(notificationDto);
        await realtimeService.BroadcastEntityChanged("Notification", "Updated", notification);
        return Ok(notification);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteNotification(int id)
    {
        await notificationService.DeleteNotification(id);
        await realtimeService.BroadcastEntityChanged("Notification", "Deleted", new { id });
        return Ok();
    }
}
