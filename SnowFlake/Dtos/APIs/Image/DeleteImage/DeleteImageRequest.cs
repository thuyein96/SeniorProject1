namespace SnowFlake.Dtos.APIs.Image.DeleteImage;

public class DeleteImageRequest
{
    public string Id { get; set; }
    public string ContainerName { get; set; }
    public string BlobName { get; set; }

}
