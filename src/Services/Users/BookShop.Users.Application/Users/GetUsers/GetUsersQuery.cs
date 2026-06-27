using BuildingBlocks.Application.CQRS;

namespace BookShop.Users.Application.Users.GetUsers;

public sealed record GetUsersQuery(
) : IQuery<IReadOnlyCollection<UserResponse>>;
