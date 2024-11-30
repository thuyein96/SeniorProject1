namespace SnowFlake.Azure.BlobsStorageService
{
    public interface IBlobStorageService
    {
        Task<string> UploadBlobAsync(string containerName, string blobName, Stream content);
        Task<bool> DeleteBlobAsync(string containerName, string blobName);
        Task<Stream> DownloadBlobAsync(string containerName, string blobName);
    }
}
