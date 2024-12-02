using MongoDB.Bson;

namespace SnowFlake.Dtos.APIs.Image.CreateImage;

public class CreateImageRequest
{
    public ObjectId Id { get; set; }
    public string FileName { get; set; }
    public Stream ImageByteData { get; set; }
    public string TeamId { get; set; }
}
