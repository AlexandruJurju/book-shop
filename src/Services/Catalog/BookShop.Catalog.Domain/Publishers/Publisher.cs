using Ardalis.Result;
using BuildingBlocks.Domain;

namespace BookShop.Catalog.Domain.Publishers;

public sealed class Publisher : Entity, IAggregateRoot
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string? Website { get; private set; }

    private Publisher() { } // EF Core

    private Publisher(Guid id, string name, string? description, string? website)
        : base(id)
    {
        Name = name;
        Description = description;
        Website = website;
    }

    public static Result<Publisher> Create(string name, string? description = null, string? website = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Error(PublisherErrors.NameRequired);
        }

        return new Publisher(Guid.CreateVersion7(), name.Trim(), description, website);
    }

    public Result Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Error(PublisherErrors.NameRequired);
        }

        Name = name.Trim();

        return Result.Success();
    }

    public Result UpdateDescription(string? description)
    {
        Description = description;
        return Result.Success();
    }

    public Result UpdateWebsite(string? website)
    {
        Website = website;
        return Result.Success();
    }
}
