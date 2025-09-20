using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;

namespace InterviewPrep.Web.Services.Email;

public class DebugEmailSender : IEmailSender
{
    private readonly ILogger<DebugEmailSender> _logger;

    public DebugEmailSender(ILogger<DebugEmailSender> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        _logger.LogInformation("Email captured for debugging. To: {Email}, Subject: {Subject}, BodyLength: {Length}", email, subject, htmlMessage?.Length ?? 0);
        return Task.CompletedTask;
    }
}
