namespace FileShare.ViewModels.Files;

public class FileViewModel
{
    public string Name { get; set; } = default!;
    public string? Comment { get; set; }
    public IFormFile File { get; set; } = default!;
}
