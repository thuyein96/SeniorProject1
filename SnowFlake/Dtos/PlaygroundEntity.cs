using MongoDB.EntityFrameworkCore;

namespace SnowFlake.Dtos;

[Collection("Playground")]
public class PlaygroundEntity : BaseEntity
{
    public string HostRoomCode { get; set; }
    public string PlayerRoomCode { get; set; }
    public string HostId { get; set; }
    public int MaxTeam { get; set; }
    public int TeamToken   { get; set; }

    public List<RoundEntity> Rounds { get; set; }

}
