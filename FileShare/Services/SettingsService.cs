using FileShare.Enums;
using FileShare.Models;

namespace FileShare.Services;

public class SettingsService : BaseService<Setting>, ISettingsService
{
    public SettingsService(DataContext dataContext) : base(dataContext)
    { }

    public string? GetSettingValue(SettingType type)
    {
        var setting = GetFirstOrDefault(s => s.Type == type);
        return setting?.Value;
    }
}

public interface ISettingsService : IBaseService<Setting>
{
    string? GetSettingValue(SettingType type);
}
