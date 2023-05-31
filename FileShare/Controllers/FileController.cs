using AutoMapper;
using FileShare.Dto;
using FileShare.Enums;
using FileShare.Helpers;
using FileShare.Services;
using FileShare.ViewModels.Files;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ImageMagick;

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
    public IActionResult AddFile([FromForm] FileViewModel model)
    {
        //Note: Consider making attributes for validation
        //Extensions validation
        var allowedExtensions = _settingsService.GetSettingValue(SettingType.FileExtensions)?.Split(", ");
        var fileInfo = new FileInfo(model.File.FileName);
        if (allowedExtensions != null && !allowedExtensions.Contains(fileInfo.Extension))
        {
            throw new AppException("Uploaded file extension is not allowed");
        }

        //Max file size validation
        var allowedFileSize = _settingsService.GetSettingValue(SettingType.MaxFileSize);
        if(allowedFileSize != null)
        {
            if(int.TryParse(allowedFileSize, out var maxFileSize)){
                if (model.File.Length > maxFileSize * 1024 * 1024)
                {
                    throw new AppException($"Uploaded file size exceeds allowed file size ({maxFileSize}MB)");
                }
            }
        }

        Stream? resizedImageReadStream = null;
        //Image size validaiton
        if (model.File.ContentType.Contains("image"))
        {
            var image = new MagickImage(model.File.OpenReadStream());
            var maxSize = new Size(_settingsService.GetSettingValue(SettingType.MaxImageSize));
            var minSize = new Size(_settingsService.GetSettingValue(SettingType.MinImageSize));
            if(minSize.Valid && (image.Width < minSize.Width || image.Height < minSize.Height))
            {
                throw new AppException("Image size does not reach minimal allowed image size");
            }
            if(maxSize.Valid && (image.Width > maxSize.Width || image.Height > maxSize.Height)){
                image.Resize(maxSize.Width, maxSize.Height);
                resizedImageReadStream = new MemoryStream(image.ToByteArray());
            }
        }

        var file = _mapper.Map<Models.File>(model);
        
        file.Extension = fileInfo.Extension;
        file.AzureFileName = AzureFileName(file.Name, file.Extension);
        file.OwnerUserId = user.Id;
        _fileService.Add(file);
        _azureStorageHelper.UploadFile(resizedImageReadStream ?? model.File.OpenReadStream(), file.AzureFileName);
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
        FileFormData formData;
        if (id.HasValue)
        {
            var file = _fileService.FindByIdOrDefault(id.Value) ?? throw new AppException("File with given id was not found");
            formData = _mapper.Map<FileFormData>(file);
        }
        else
        {
            formData = new();
            var minImageSize = new Size(_settingsService.GetSettingValue(SettingType.MinImageSize));
            if(minImageSize.Valid)
            {
                formData.MinSize = minImageSize;
            }
            if (int.TryParse(_settingsService.GetSettingValue(SettingType.MaxFileSize), out int maxFileSize))
            {
                formData.MaxFileSize = maxFileSize;
            }
            formData.AllowedExtensions = _settingsService.GetSettingValue(SettingType.FileExtensions);
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
