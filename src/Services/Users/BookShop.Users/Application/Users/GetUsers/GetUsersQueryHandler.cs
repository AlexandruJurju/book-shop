using Ardalis.Result;
using BookShop.Users.Application.Abstractions.Data;
using BuildingBlocks.Application.CQRS;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Users.Application.Users.GetUsers;

public sealed class GetUsersQueryHandler(
    IUsersDbContext dbContext
) : IQueryHandler<GetUsersQuery, IReadOnlyCollection<UserResponse>>
{
    public async ValueTask<Result<IReadOnlyCollection<UserResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        List<UserResponse> users = await dbContext.Users
            .Select(x => new UserResponse(x.Id, x.UserName, x.Email))
            .ToListAsync(cancellationToken);

        return users;
    }
}
