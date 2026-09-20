using Ardalis.Result;
using BookShop.Users.Application.Abstractions.Data;
using BookShop.Users.Application.Abstractions.Identity;
using BookShop.Users.Domain.Users;
using BuildingBlocks.Application.CQRS;
using FluentValidation;

namespace BookShop.Users.Application.Users;

public sealed record RegisterUserCommand(
    string UserName,
    string Email,
    string Password
) : ICommand<Guid>;

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(c => c.UserName)
            .NotEmpty();

        RuleFor(c => c.Email)
            .EmailAddress();

        RuleFor(c => c.Password)
            .MinimumLength(8);
    }
}

public sealed class RegisterUserCommandHandler(
    IUsersDbContext usersDbContext,
    IUnitOfWork unitOfWork,
    IIdentityProvider identityProvider
) : ICommandHandler<RegisterUserCommand, Guid>
{
    public async ValueTask<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        Result<string> result = await identityProvider.CreateUserAsync(
            new UserModel(request.UserName, request.Email, request.Password),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return Result.Error();
        }

        var user = User.Create(
            request.UserName,
            Email.Create(request.Email),
            result.Value
        );

        usersDbContext.Users.Add(user);
        usersDbContext.Roles.Attach(Role.Member);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
