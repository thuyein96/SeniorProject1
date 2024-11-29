using MongoDB.EntityFrameworkCore;

namespace SnowFlake.Dtos;

[Collection("Playground")]
public class PlaygroundEntity : BaseEntity
{
    public string RoomCode { get; set; }
    public string PlayerId { get; set; }
    public List<RoundEntity> Rounds { get; set; }
    public int MaxTeam { get; set; }
    public int TeamToken   { get; set; }

}
