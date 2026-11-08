namespace BookShop.Users.Domain.Users;

public sealed class Permission
{
    public static readonly Permission ReadUser = new(1, "users:read");
    public static readonly Permission UpdateUser = new(2, "users:update");

    private Permission(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; init; }

    public string Name { get; init; }
}
