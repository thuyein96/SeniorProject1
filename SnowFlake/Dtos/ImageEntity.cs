using MongoDB.EntityFrameworkCore;

namespace SnowFlake.Dtos;

[Collection("Images")]
public class ImageEntity : BaseEntity
{
    public string SnowFlakeImageUrl { get; set; }
    public string ImageBuyingStatus { get; set; }
    public string TeamId { get; set; }
}
