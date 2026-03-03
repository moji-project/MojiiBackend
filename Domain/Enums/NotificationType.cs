namespace MojiiBackend.Domain.Enums;

public enum NotificationType
{
    NewEventCreated,
    EventUpdated,
    NewMessage,
    NewCommentOnPost,
    NewLikeOnPost,
    PostReported,
    CommentReported,
    StudentReported,
    AccountActivated,
    AccountSuspended,
    Other
}