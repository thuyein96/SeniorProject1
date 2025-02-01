using MongoDB.EntityFrameworkCore;

namespace SnowFlake.Dtos;

[Collection("Images")]
public class ImageEntity : BaseEntity
{
    public string FileName { get; set; }
    public string SnowFlakeImageUrl { get; set; }
    public string ImageBuyingStatus { get; set; }
    public string OwnerId { get; set; }
    public int? Price { get; set; }
}
