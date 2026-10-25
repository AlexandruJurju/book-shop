using Ardalis.Result;
using BuildingBlocks.Domain;

namespace BookShop.Catalog.Domain.Publishers;

public sealed class Publisher : Entity<Guid>, IAggregateRoot
{
    public string Name { get; private set; }

    private Publisher() { } // EF Core

    private Publisher(Guid id, string name)
        : base(id)
    {
        Name = name;
    }

    public static Result<Publisher> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Error(PublisherErrors.NameRequired);
        }

        return new Publisher(Guid.CreateVersion7(), name.Trim());
    }
}
