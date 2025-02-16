using MongoDB.EntityFrameworkCore;

namespace SnowFlake.Dtos;

[Collection("Shop")]
public class ShopEntity : BaseEntity
{
    public string HostRoomCode { get; set; }
    public string PlayerRoomCode { get; set; }
    public int Tokens { get; set; }
}
