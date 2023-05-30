using FileShare.Dto;

namespace FileShare.ViewModels.Files;

public class FileFormData
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Comment { get; set; }
    public string? Extension { get; set; }
    public Size? MinSize { get; set; }
    public int? MaxFileSize { get; set; }
    public string? AllowedExtensions { get; set; }
    
}
