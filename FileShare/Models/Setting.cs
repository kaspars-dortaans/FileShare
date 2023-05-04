namespace FileShare.Models;

public class Setting
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Value { get; set; } = default!;
}
