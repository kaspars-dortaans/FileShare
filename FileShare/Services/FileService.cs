using FileShare.Models;

namespace FileShare.Services;

public class FileService: BaseService<Models.File>, IFileService
{
    public FileService(DataContext context): base(context)
    { }
}

public interface IFileService : IBaseService<Models.File>
{ }
