using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace VideoPlayerAPI.Infrastructure.AzureStorage;

public interface IAzureStorageHelper
{
    Task<string> UploadFileAsync(byte[] fileBytes, string path);
    Task<string> UploadFileAsync(Stream stream, string path, string contentType);
    Task<IList<string>> GetFileListAsync(string path);
    Task DeleteFileAsync(string path);
    Task DeleteDirectoryAsync(string path);
    string GetFilePath(string filePath);
    Task<bool> CheckIfBlobExistsAsync(string path);
    Task<byte[]> GetFileAsync(string path);
    Task<Stream> GetFileByStreamAsync(string path);
}

public class AzureStorageHelper : IAzureStorageHelper
{
    private readonly ILogger<AzureStorageHelper> _logger;
    private readonly string _connectionString;
    private readonly string _containerName;
    private readonly string _baseUrl;

    public AzureStorageHelper(IConfiguration configuration, ILogger<AzureStorageHelper> logger)
    {
        _logger = logger;
        _connectionString = configuration["AzureStorage:ConnectionString"];
        _baseUrl = configuration["AzureStorage:BaseUrl"];
        _containerName = configuration["AzureStorage:ContainerName"];
    }

    public async Task<string> UploadFileAsync(byte[] fileBytes, string path)
    {
        try
        {
            var containerClient = GetContainer();
            var relativePath = GetRelativePath(containerClient.Uri.ToString(), path);
            var blobClient = containerClient.GetBlobClient(relativePath);

            await blobClient.UploadAsync(new BinaryData(fileBytes), true);

            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during uploading file to Azure Storage");

            throw new Exception("Error uploading file");
        }
    }

    public async Task<string> UploadFileAsync(Stream stream, string path, string contentType)
    {
        try
        {
            var containerClient = GetContainer();
            var relativePath = GetRelativePath(containerClient.Uri.ToString(), path);
            var blobClient = containerClient.GetBlobClient(relativePath);

            var options = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = contentType
                }
            };

            stream.Position = 0;

            await blobClient.UploadAsync(stream, options, default);

            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during uploading file to Azure Storage");

            throw new Exception("Error uploading file");
        }
    }

    public async Task<IList<string>> GetFileListAsync(string path)
    {
        try
        {
            var containerClient = GetContainer();

            var results = await ListBlobsHierarchicalListing(containerClient, path);

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during fetching files from Azure Storage");

            throw new Exception("Error fetching file");
        }
    }

    public async Task DeleteFileAsync(string path)
    {
        try
        {
            var containerClient = GetContainer();
            var relativePath = GetRelativePath(containerClient.Uri.ToString(), path);
            var blobClient = containerClient.GetBlobClient(relativePath);

            await blobClient.DeleteIfExistsAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during deleting file from Azure Storage");

            throw new Exception("Error deleting file");
        }
    }

    public async Task DeleteDirectoryAsync(string path)
    {
        try
        {
            var containerClient = GetContainer();
            var relativePath = GetRelativePath(containerClient.Uri.ToString(), path);

            var blobItems = containerClient.GetBlobsAsync(prefix: relativePath);

            await foreach (var blobItem in blobItems)
            {
                var blobClient = containerClient.GetBlobClient(blobItem.Name);
                _ = await blobClient.DeleteIfExistsAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during deleting directory from Azure Storage");

            throw new Exception("Error deleting directory");
        }
    }

    public string GetFilePath(string filePath)
    {
        var uriBuilder = new UriBuilder(_baseUrl);
        uriBuilder.Path += Path.Combine(_containerName, filePath);

        return uriBuilder.Uri.ToString();
    }

    public async Task<bool> CheckIfBlobExistsAsync(string path)
    {
        var containerClient = GetContainer();
        var relativePath = GetRelativePath(containerClient.Uri.ToString(), path);
        var blobClient = containerClient.GetBlobClient(relativePath);
        var blobExists = await blobClient.ExistsAsync();

        return blobExists;
    }

    public async Task<byte[]> GetFileAsync(string path)
    {
        try
        {
            var containerClient = GetContainer();
            var relativePath = GetRelativePath(containerClient.Uri.ToString(), path);
            var blobClient = containerClient.GetBlobClient(relativePath);

            BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();

            var blobContents = downloadResult.Content.ToArray();

            return blobContents;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during fetching file from Azure Storage");

            throw new Exception("Error fetching file");
        }
    }

    public async Task<Stream> GetFileByStreamAsync(string path)
    {
        try
        {
            var containerClient = GetContainer();
            var relativePath = GetRelativePath(containerClient.Uri.ToString(), path);
            var blobClient = containerClient.GetBlobClient(relativePath);

            var memoryStream = new MemoryStream();

            await blobClient.DownloadToAsync(memoryStream);

            memoryStream.Position = 0;

            return memoryStream;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during fetching file using stream from Azure Storage");

            throw new Exception("Error fetching file");
        }
    }

    private async Task<List<string>> ListBlobsHierarchicalListing(BlobContainerClient container, string path)
    {
        var resultSegment = container
            .GetBlobsByHierarchyAsync(prefix: path, delimiter: "/")
            .AsPages();

        var results = new List<string>();

        await foreach (var blobPage in resultSegment)
        {
            foreach (var item in blobPage.Values)
            {
                if (item.IsPrefix)
                {
                    results.AddRange(await ListBlobsHierarchicalListing(container, item.Prefix));
                }
                else
                {
                    results.Add(GetFilePath(item.Blob.Name));
                }
            }
        }

        return results;
    }

    private BlobContainerClient GetContainer()
    {
        var blobServiceClient = new BlobServiceClient(_connectionString);
        return blobServiceClient.GetBlobContainerClient(_containerName);
    }

    private static string GetRelativePath(string blobParentPath, string path)
    {
        return path.Replace($"{blobParentPath}/", string.Empty);
    }
}
