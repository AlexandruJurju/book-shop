using Ardalis.Result;
using BuildingBlocks.Application.Mediator;

namespace BookShop.Catalog.Application.Categories.CreateCategory;

internal sealed class CreateCategoryCommandHandler(
) : ICommandHandler<CreateCategoryCommand, Guid>
{
    public async ValueTask<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
