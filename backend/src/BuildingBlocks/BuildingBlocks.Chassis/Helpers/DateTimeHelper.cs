namespace BuildingBlocks.Chassis.Helpers;

public static class DateTimeHelper
{
    public static DateTime UtcNow => TimeProvider.System.GetUtcNow().UtcDateTime;
}
