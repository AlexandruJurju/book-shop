namespace BookShop.Users.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(User user);
    Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default);
}
