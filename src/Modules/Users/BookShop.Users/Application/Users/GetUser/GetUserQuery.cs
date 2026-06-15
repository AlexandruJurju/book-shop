using BuildingBlocks.Application.CQRS;

namespace BookShop.Users.Application.Users.GetUser;

public sealed record GetUserQuery(
    Guid UserId
) : IQuery<UserResponse>;
