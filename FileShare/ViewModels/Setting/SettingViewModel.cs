using FileShare.Enums;
using System.ComponentModel.DataAnnotations;

namespace FileShare.ViewModels.Setting;

public class SettingViewModel
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Value { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public SettingDataType DataType { get; set; } = default!;
}
