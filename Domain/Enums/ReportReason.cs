namespace MojiiBackend.Domain.Enums;

public enum ReportReason
{
    InapropriateContent = 0,
    Spam = 1,
    Harassment = 2,
    FakeInformation = 3,
    Other = 4,

    [Obsolete("Legacy value kept for backward compatibility with existing records.")]
    HateSpeech = 5,

    [Obsolete("Legacy value kept for backward compatibility with existing records.")]
    IncitationToTerrorism = 6
}
