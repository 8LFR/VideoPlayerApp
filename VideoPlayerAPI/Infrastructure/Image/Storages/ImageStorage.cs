using VideoPlayerAPI.Infrastructure.AzureStorage;
using VideoPlayerAPI.Infrastructure.Image.Models;

namespace VideoPlayerAPI.Infrastructure.Image.Storages;

public interface IImageStorage
{
    Task<string> AddAsync(byte[] data, string path);
    Task<IList<string>> GetListAsync(string path);
    Task DeleteByUriAsync(string path);
    Task DeleteAsync(string path);
    Task DeleteDirectoryAsync(string path);
    string GetImageUrl(string path);
    ImageData GetImageAsync(string path);
}

public class ImageStorage(IAzureStorageHelper storageHelper) : IImageStorage
{
    private readonly IAzureStorageHelper _storageHelper = storageHelper;

    public Task<string> AddAsync(byte[] data, string path) => _storageHelper.UploadFileAsync(data, $"Images/{path}");

    public Task<IList<string>> GetListAsync(string path) => _storageHelper.GetFileListAsync($"Images/{path}");

    public Task DeleteAsync(string path) => _storageHelper.DeleteFileAsync($"Images/{path}");

    public Task DeleteByUriAsync(string path) => _storageHelper.DeleteFileAsync(path);

    public Task DeleteDirectoryAsync(string path) => _storageHelper.DeleteDirectoryAsync($"Images/{path}");

    public string GetImageUrl(string path) => _storageHelper.GetFilePath($"Images/{path}");

    public ImageData GetImageAsync(string path)
    {
        var blobBytes = _storageHelper.GetFileAsync(path).Result;

        return new ImageData
        {
            ImageType = Path.GetExtension(path),
            Bytes = blobBytes
        };
    }
}
