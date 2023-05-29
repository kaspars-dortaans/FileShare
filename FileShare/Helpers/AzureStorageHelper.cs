using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;

namespace FileShare.Helpers;

public class AzureStorageHelper : IAzureStorageHelper
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IOptions<AppSettings> _appSettings;

    public AzureStorageHelper(IOptions<AppSettings> appSettings, BlobServiceClient blobStorageClient)
    {
        _blobServiceClient = blobStorageClient;
        _appSettings = appSettings;
    }

    public Stream? GetFileStreamOrDefault(string name, string? container = null)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(container ?? _appSettings.Value.DefaultAzureStorageContainer);
        var blobClient = blobContainerClient.GetBlobClient(name);
        if (!blobClient.Exists().Value)
        {
            return null;
        }
            
        var response = blobClient.Download();
        return response.Value.Content;
    }

    public void UploadFile(Stream fileStream, string name, string? container = null)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(container ?? _appSettings.Value.DefaultAzureStorageContainer);
        blobContainerClient.UploadBlob(name, fileStream);
    }

    public void DeleteFile(string name, string? container = null)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(container ?? _appSettings.Value.DefaultAzureStorageContainer);
        blobContainerClient.DeleteBlob(name);
    }
}

public interface IAzureStorageHelper
{
    Stream? GetFileStreamOrDefault(string name, string? container = null);
    void UploadFile(Stream fileStream, string name, string? container = null);
    void DeleteFile(string name, string? container = null);
}
