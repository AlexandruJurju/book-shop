namespace BuildingBlocks.Shared.Helpers;

public static class GuidHelper
{
    public static Guid NewGuid()
    {
        return Guid.CreateVersion7();
    }
}
