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
    public async Task<Result<UserResponse>> HandleAsync(GetUserQuery query, CancellationToken cancellationToken)
    {
        UserResponse? user = await usersDbContext.Users
            .Where(user => user.Id == query.UserId)
            .Select(user => new UserResponse(user.Id, user.Email))
            .SingleOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            return Result.NotFound(UserErrors.NotFound(query.UserId));
        }

        return user;
    }
}
