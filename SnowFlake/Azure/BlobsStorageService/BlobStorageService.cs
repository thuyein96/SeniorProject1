using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace SnowFlake.Azure.BlobsStorageService
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<string> UploadBlobAsync(string containerName, string blobName, Stream content)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(content, overwrite: true);
            containerClient.SetAccessPolicy(PublicAccessType.Blob);

            return blobClient.Uri.ToString();
        }

        public async Task<Stream> DownloadBlobAsync(string containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            var response = await blobClient.DownloadAsync();
            return response.Value.Content;
        }

        public async Task<bool> DeleteBlobAsync(string containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            return await blobClient.DeleteIfExistsAsync();
        }
    }
}
