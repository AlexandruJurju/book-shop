using Ardalis.Result;
using BookShop.Users.Application.Abstractions.Data;
using BookShop.Users.Application.Abstractions.Identity;
using BookShop.Users.Domain.Users;
using BuildingBlocks.Application.CQRS;
using BuildingBlocks.Common.Extensions;

namespace BookShop.Users.Application.Users.RegisterUser;

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

        if (result.IsFailure)
        {
            return Result.Error();
        }

        var user = User.Create(
            request.UserName,
            request.Email,
            result.Value
        );

        usersDbContext.Users.Add(user);
        usersDbContext.Roles.Attach(Role.Registered);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
