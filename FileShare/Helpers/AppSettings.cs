namespace FileShare.Helpers;

public class AppSettings
{
    public string Secret { get; set; } = default!;
    public string DefaultAzureStorageContainer { get; set; } = default!;
}