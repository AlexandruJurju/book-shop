using Ardalis.Result;
using BookShop.Users.Application.Abstractions.Data;
using BookShop.Users.Domain.Users;
using BuildingBlocks.Application.CQRS;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Users.Application.Users.GetUser;

public sealed class GetUserQueryHandler(
    IUsersDbContext usersDbContext
) : IQueryHandler<GetUserQuery, UserResponse>
{
    public async ValueTask<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        UserResponse? user = await usersDbContext.Users
            .Where(user => user.Id == request.UserId)
            .Select(user => new UserResponse(user.Id, user.Email))
            .SingleOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            return Result.NotFound(UserErrors.NotFound(request.UserId));
        }

        return user;
    }
}
