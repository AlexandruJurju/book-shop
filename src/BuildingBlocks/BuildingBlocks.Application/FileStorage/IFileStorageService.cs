using Ardalis.Result;

namespace BuildingBlocks.Application.FileStorage;

public interface IFileStorageService
{
    Task<Result> UploadAsync(string key, byte[] data, string contentType, CancellationToken ct);
    Task<Result> DeleteAsync(string key, CancellationToken ct);
    Task<bool> ExistsAsync(string key, CancellationToken ct);
    Task<Result<FileDownloadOutput>> DownloadAsync(string key, CancellationToken ct);
}
