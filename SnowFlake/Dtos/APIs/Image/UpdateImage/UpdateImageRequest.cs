using MongoDB.Bson;

namespace SnowFlake.Dtos.APIs.Image.UpdateImage;

public class UpdateImageRequest
{
    public string Id { get; set; }
    public string? OldImageFileName { get; set; }
    public string? NewImageFileName { get; set; }
    public Stream? NewImageByteData { get; set; }
    public string? ImageBuyingStatus { get; set; }
    public string TeamId { get; set; }
    public DateTime CreationDate { get; set; }
}
