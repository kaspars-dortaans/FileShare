using FileShare.Enums;

namespace FileShare.ViewModels.Setting;

public class SettingViewModel
{
    public int Id { get; set; }
    public string Value { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public SettingDataType DataType { get; set; } = default!;
}
