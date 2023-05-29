using AutoMapper;
using AutoMapper.Configuration.Conventions;
using FileShare.Dto;
using FileShare.Enums;
using FileShare.Helpers;
using FileShare.Services;
using FileShare.ViewModels.Files;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Linq;

namespace FileShare.Controllers;

public class FileController : BaseController
{
    private readonly IAzureStorageHelper _azureStorageHelper;
    private readonly IFileService _fileService;
    private readonly ISettingsService _settingsService;
    private readonly IMapper _mapper;

    public FileController(IAzureStorageHelper azureStorageHelper, IFileService fileService, ISettingsService settingsService, IMapper mapper) : base()
    {
        _azureStorageHelper = azureStorageHelper;
        _fileService = fileService;
        _settingsService = settingsService;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetFiles(int? currentPage, int? pageSize)
    {
        var files = _fileService.Get(file => file.OwnerUserId == user.Id);
        if(currentPage.HasValue && pageSize.HasValue)
        {
            files = files
                .Skip((currentPage.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);
        }
        return Ok(_mapper.Map<IEnumerable<FileListItem>>(files));
    }

    [HttpPost]
    public IActionResult AddFIle([FromForm] FileViewModel model)
    {
        if(model.File == null)
        {
            throw new AppException("Required file was not received");
        }
        var file = _mapper.Map<Models.File>(model);
        var fileInfo = new FileInfo(model.File.FileName);
        file.Extension = fileInfo.Extension;
        file.AzureFileName = AzureFileName(file.Name, file.Extension);
        file.OwnerUserId = user.Id;
        _fileService.Add(file);
        _azureStorageHelper.UploadFile(model.File.OpenReadStream(), file.AzureFileName);
        return Ok();
    }

    [HttpGet]
    public IActionResult DownloadFile(int id)
    {
        var file = _fileService.FindByIdOrDefault(id) ?? throw new AppException("File with given id was not found");
        var fileStream = _azureStorageHelper.GetFileStreamOrDefault(file.AzureFileName) ?? throw new AppException("Document was not found");
        var cd = new System.Net.Mime.ContentDisposition
        {
            FileName = $"{file.Name}{file.Extension}",
            DispositionType = "attachment"
        };
        Response.Headers.Add(HeaderNames.ContentDisposition, cd.ToString());
        return File(fileStream, "application/octet-stream");
    }

    [HttpGet]
    public IActionResult GetFileFormData(int? id)
    {
        FileViewModel formData;
        if (id.HasValue)
        {
            var file = _fileService.FindByIdOrDefault(id.Value) ?? throw new AppException("File with given id was not found");
            formData = _mapper.Map<FileViewModel>(file);
        }
        else
        {
            formData = new();
            formData.MinSize = new Size(_settingsService.GetSettingValue(SettingType.MinImageSize));
            if (int.TryParse(_settingsService.GetSettingValue(SettingType.MaxFileSize), out int maxFileSize))
            {
                formData.MaxFileSize = maxFileSize;
            }   
        }
        return Ok(formData);
    }

    [HttpPost]
    public IActionResult EditFile(FileListItem model)
    {
        var file = _fileService.FindByIdOrDefault(model.Id) ?? throw new AppException("File with given id was not found");
        file.Name = model.Name;
        file.Comment = model.Comment;
        _fileService.Update(file);

        return Ok();
    }

    [HttpDelete]
    public IActionResult DeleteFile(int id)
    {
        var file = _fileService.FindByIdOrDefault(id) ?? throw new AppException("File with given id was not found");
        _azureStorageHelper.DeleteFile(file.AzureFileName);
        _fileService.Remove(file);

        return Ok();
    }

    //private methods
    private string AzureFileName(string name, string extension)
    {
        return $"{user.Id}/{new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()}-{name}{extension}";
    }
}
