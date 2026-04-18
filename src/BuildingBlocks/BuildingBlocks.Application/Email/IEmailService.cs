using Ardalis.Result;

namespace BuildingBlocks.Application.Email;

public interface IEmailService
{
    Task<Result> SendEmailAsync(EmailMessage message, CancellationToken cancellationToken = default);
}
