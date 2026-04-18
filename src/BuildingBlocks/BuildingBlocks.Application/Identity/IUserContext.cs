namespace BuildingBlocks.Application.Identity;

public interface IUserContext
{
    Guid? UserId { get; }
    string? Email { get; }
    string? UserName { get; }
}
