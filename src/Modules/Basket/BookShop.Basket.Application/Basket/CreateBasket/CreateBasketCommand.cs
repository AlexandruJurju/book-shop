using BuildingBlocks.Application.CQRS;

namespace BookShop.Basket.Application.Basket.CreateBasket;

public sealed record CreateBasketCommand : ICommand<Guid>;
