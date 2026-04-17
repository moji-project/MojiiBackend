using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MojiiBackend.API.Hubs;

[Authorize]
public class RealtimeHub : Hub
{
    public Task Ping() => Task.CompletedTask;
}
