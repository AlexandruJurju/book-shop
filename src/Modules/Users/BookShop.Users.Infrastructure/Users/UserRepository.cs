using BookShop.Users.Domain.Users;
using BookShop.Users.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Users.Infrastructure.Users;

internal sealed class UserRepository(
    UsersDbContext dbContext
) : IUserRepository
{
    public async Task<User?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AnyAsync(u => u.Email == new Email(email), cancellationToken);
    }

    public void Add(User user)
    {
        dbContext.Users
            .Add(user);
    }
}
