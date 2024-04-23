using VideoPlayerAPI.Infrastructure.AzureStorage;

namespace VideoPlayerAPI.Infrastructure.Video.Storages;

public interface IVideoStorage
{
    Task<string> AddAsync(byte[] data, string filename);
    Task DeleteAsync(string path);
    string GetVideoUrl(string filename);
    VideoData GetVideoAsync(string path);
}

public class VideoStorage : IVideoStorage
{
    private readonly IAzureStorageHelper _storageHelper;

    public VideoStorage(IAzureStorageHelper storageHelper)
    {
        _storageHelper = storageHelper;
    }

    public Task<string> AddAsync(byte[] data, string path) => _storageHelper.UploadFileAsync(data, $"Videos/{path}");
    public Task DeleteAsync(string path) => _storageHelper.DeleteFileAsync($"Videos/{path}");
    public string GetVideoUrl(string filename) => _storageHelper.GetFilePath($"Videos/{filename}");
    public VideoData GetVideoAsync(string path)
    {
        var blobBytes = _storageHelper.GetFileAsync(path).Result;

        return new VideoData
        {
            VideoType = Path.GetExtension(path),
            Bytes = blobBytes
        };
    }
}
