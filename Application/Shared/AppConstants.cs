namespace MojiiBackend.Application.Shared;

public static class AppConstants
{
    public const string DirectChannelPrefix = "__dm__";
    public const string ChatHubRoute = "/hubs/chat";
    public const string RealtimeHubRoute = "/hubs/realtime";
    public const string HubsRoutePrefix = "/hubs";
}

public static class AppRoles
{
    public const string Student = "Student";
    public const string SchoolAdmin = "SchoolAdmin";
    public const string SuperAdmin = "SuperAdmin";
}
