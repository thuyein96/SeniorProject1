using MongoDB.EntityFrameworkCore;

namespace SnowFlake.Dtos;

[Collection("Playground")]
public class PlaygroundEntity : BaseEntity
{
    public string HostRoomCode { get; set; }
    public string PlayerRoomCode { get; set; }
    public int NumberOfTeam { get; set; }
    public int TeamToken { get; set; }
    public List<RoundEntity> Rounds { get; set; }
    public List<Product> Shop { get; set; }

}
