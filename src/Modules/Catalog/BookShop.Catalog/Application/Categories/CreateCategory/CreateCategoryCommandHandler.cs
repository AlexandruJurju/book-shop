using Ardalis.Result;
using BuildingBlocks.Application.CQRS;

namespace BookShop.Catalog.Application.Categories.CreateCategory;

// todo: make this internal
public sealed class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, Guid>
{
    public async ValueTask<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
