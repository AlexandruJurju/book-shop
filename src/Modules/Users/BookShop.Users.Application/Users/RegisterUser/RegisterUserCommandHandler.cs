using Ardalis.Result;
using BookShop.Users.Domain.Users;
using BuildingBlocks.Application.CQRS;
using BuildingBlocks.Application.Data;

namespace BookShop.Users.Application.Users.RegisterUser;

public sealed class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<RegisterUserCommand, Guid>
{
    public async ValueTask<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.ExistsAsync(request.Email, cancellationToken))
        {
            return UserErrors.NotFound(request.Email).ToResult<Guid>();
        }

        var user = User.Create(
            new UserName(request.UserName),
            new Email(request.Email)
        );

        userRepository.Add(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
