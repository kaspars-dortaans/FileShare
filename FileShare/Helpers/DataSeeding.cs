using FileShare.Models;
using FileShare.ViewModels.Setting;

namespace FileShare.Helpers;

public class DataSeeding
{
    private readonly DataContext _context;

    public DataSeeding(DataContext dataContext)
    {
        _context = dataContext;
    }

    public void SeedData()
    {
        var settingsInDb = _context.Settings.ToList();
        var settingsToAdd = Enums.AppSettings.Settings.Where(setting => !settingsInDb.Any(settingInDb => settingInDb.Type == setting.Key));
        if (settingsToAdd.Any())
        {
            var newSettomgs = settingsToAdd.Select(setting =>
                    {
                        return new Setting()
                        {
                            Type = setting.Key,
                            DataType = setting.Value.dataType,
                            Description = string.Empty,
                            Value = string.Empty
                        };
                    });
            _context.AddRange(newSettomgs);
            _context.SaveChanges();
        }

    }
}
