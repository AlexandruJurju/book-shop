namespace BookShop.Shared;

public static class Azure
{
    public static class Storage
    {
        public const string Resource = "storage";

        public static string BlobContainer(string containerName)
        {
            return $"{containerName}-blob";
        }
    }
}
