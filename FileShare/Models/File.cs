namespace FileShare.Models;

public class File
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Extension { get; set; } = default!;
    public string? Comment { get; set; }
    public string AzureFileName { get; set; } = default!;
    public int OwnerUserId { get; set; }
}
