using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Repositories;
using MojiiBackend.Infrastructure.Emailing;

namespace MojiiBackend.Application.Services;

public class FeedbackService(
    EmailService emailService,
    ICurrentUserService currentUserService,
    UserRepository userRepository,
    IConfiguration configuration)
{
    private readonly string _feedbackRecipientEmail =
        configuration["FeedbackSettings:RecipientEmail"] ?? "lukas.bouhlel@ynov.com";

    public async Task SendFeedback(CreateFeedbackRequestDto createFeedbackRequestDto)
    {
        var subject = createFeedbackRequestDto.Subject?.Trim() ?? string.Empty;
        var message = createFeedbackRequestDto.Message?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(subject))
            throw new ArgumentException("Subject is required.");

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message is required.");

        if (subject.Length > 120)
            throw new ArgumentException("Subject cannot exceed 120 characters.");

        if (message.Length > 3000)
            throw new ArgumentException("Message cannot exceed 3000 characters.");

        var senderUserId = currentUserService.GetUserId();
        var senderUser = await userRepository.GetById(senderUserId);
        if (senderUser is null)
            throw new UnauthorizedAccessException("User not authenticated.");

        await emailService.SendFeedbackEmailAsync(
            _feedbackRecipientEmail,
            subject,
            message,
            senderUserId,
            senderUser.Email,
            senderUser.FirstName,
            senderUser.LastName);
    }
}

