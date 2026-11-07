namespace BookShop.Users.UnitTests.Abstractions;

internal sealed class GlobalSetup
{
    [Before(TestDiscovery)]
    public static void Configure(BeforeTestDiscoveryContext context)
    {
        // strict behavior for mock - unconfigured calls cause exception
        context.Settings.Mocks.DefaultMode = MockBehavior.Strict;
    }
}
