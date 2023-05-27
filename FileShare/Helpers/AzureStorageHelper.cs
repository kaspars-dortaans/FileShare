using Azure.Storage.Blobs;

namespace FileShare.Helpers;

public class AzureStorageHelper : IAzureStorageHelper
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly AppSettings _appSettings;

    public AzureStorageHelper(AppSettings appSettings, BlobServiceClient blobStorageClient)
    {
        _blobServiceClient = blobStorageClient;
        _appSettings = appSettings;
    }

    public Stream GetFileStream(string name, string? container)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(container ?? _appSettings.DefaultAzureStorageContainer);
        var blobClient = blobContainerClient.GetBlobClient(name);
        var response = blobClient.Download();
        return response.Value.Content;
    }

    public void UploadFile(Stream fileStream, string name, string? container)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(container ?? _appSettings.DefaultAzureStorageContainer);
        blobContainerClient.UploadBlob(name, fileStream);
    }

    public void DeleteFile(string name, string? container)
    {
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(container ?? _appSettings.DefaultAzureStorageContainer);
        blobContainerClient.DeleteBlob(name);
    }
}

public interface IAzureStorageHelper
{
    Stream GetFileStream(string name, string? container);
    void UploadFile(Stream fileStream, string name, string? container);
    void DeleteFile(string name, string? container);
}
