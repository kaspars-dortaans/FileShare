using FileShare.Dto;

namespace FileShare.ViewModels.Files;

public class FileViewModel
{
    public int? Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Comment { get; set; }
    public IFormFile File { get; set; } = default!;
    public Size? MinSize { get; set; }
    public int? MaxFileSize { get; set; }
    public string? Extension { get; set; }
}
