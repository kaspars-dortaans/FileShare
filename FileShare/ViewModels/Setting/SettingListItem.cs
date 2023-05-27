namespace FileShare.ViewModels.Setting;

public class SettingListItem
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Value { get; set; } = default!;
    public string Description { get; set; } = default!;
}
