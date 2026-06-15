namespace BookShop.Users.Domain.Users;

public sealed class Role
{
    public static readonly Role Administrator = new(1, "Administrator");
    public static readonly Role Registered = new(2, "Member");

    public Role(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; init; }

    public string Name { get; init; }

    public ICollection<User> Users { get; init; } = new List<User>();

    public ICollection<Permission> Permissions { get; init; } = new List<Permission>();
}
