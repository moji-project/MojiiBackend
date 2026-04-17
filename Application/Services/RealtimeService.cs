using Microsoft.AspNetCore.SignalR;
using MojiiBackend.API.Hubs;
using MojiiBackend.Application.DTOs;

namespace MojiiBackend.Application.Services;

public class RealtimeService(IHubContext<RealtimeHub> hubContext)
{
    public async Task BroadcastEntityChanged(string entity, string action, object? payload = null)
    {
        var change = new RealtimeChangeDto
        {
            Entity = entity,
            Action = action,
            Payload = payload,
            OccurredAt = DateTime.UtcNow
        };

        await hubContext.Clients.All.SendAsync("EntityChanged", change);
    }
}
