
namespace FileShare.ViewModels.Setting;

public class SettingsProfile : AutoMapper.Profile
{
    public SettingsProfile()
    {
        CreateMap<Models.Setting, SettingViewModel>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Type))
            .ReverseMap();
        CreateMap<Models.Setting, SettingListItem>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Type));
    }
}
