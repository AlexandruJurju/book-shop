using Ardalis.Result;
using BookShop.Users.Application.Abstractions.Data;
using BookShop.Users.Contracts;
using BuildingBlocks.Application.CQRS;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Users.Application.Users;

public sealed record GetUsersQuery : IQuery<IReadOnlyCollection<UserResponse>>;

public sealed class GetUsersQueryHandler(
    IUsersDbContext dbContext
) : IQueryHandler<GetUsersQuery, IReadOnlyCollection<UserResponse>>
{
    public async ValueTask<Result<IReadOnlyCollection<UserResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        List<UserResponse> users = await dbContext.Users
            .Select(user => new UserResponse(user.Id, user.UserName, user.Email.Value))
            .ToListAsync(cancellationToken);

        return users;
    }
}
