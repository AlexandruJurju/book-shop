namespace BuildingBlocks.Domain;

public interface IHasConcurrency
{
    int Version { get; set; }
}
