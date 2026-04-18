namespace BuildingBlocks.Application;

public sealed class PagedResponse<T>
{
    public IReadOnlyCollection<T> Items { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }

    public int TotalPages =>
        (TotalCount + PageSize - 1) / PageSize;

    public bool HasPreviousPage
        => PageNumber > 1;

    public bool HasNextPage
        => PageNumber < TotalPages;
}
