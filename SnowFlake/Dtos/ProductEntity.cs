using MongoDB.EntityFrameworkCore;

namespace SnowFlake.Dtos;
[Collection("Product")]
public class ProductEntity : BaseEntity
{
    public string ProductName { get; set; }
    public int Price { get; set; }
    public int RemainingStock { get; set; }
    public string? OwnerId { get; set; }
}
