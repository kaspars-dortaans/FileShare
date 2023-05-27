using FileShare.Enums;

namespace FileShare.Models;

public class Setting
{
    public int Id { get; set; }
    public SettingType Type { get; set; }
    public string Description { get; set; } = default!;
    public string Value { get; set; } = default!;
    public SettingDataType DataType { get; set; }
}
