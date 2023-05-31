namespace FileShare.ViewModels.Files;

public class FilesProfile : AutoMapper.Profile
{
    public FilesProfile()
    {
        CreateMap<Models.File, FileListItem>()
            .ForMember(d => d.Name, o => o.MapFrom(s => $"{s.Name}{s.Extension}"));
        CreateMap<Models.File, FileFormData>();
        CreateMap<FileViewModel, Models.File>();
    }
}
