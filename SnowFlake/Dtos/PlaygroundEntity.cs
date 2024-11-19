using MongoDB.EntityFrameworkCore;

namespace SnowFlake.Dtos;

[Collection("Playground")]
public class PlaygroundEntity : BaseEntity
{
    public string RoomCode { get; set; }
    public Dictionary<string, TimeOnly> Rounds { get; set; }
    public int MaxTeam { get; set; }
    public int TeamToken   { get; set; }

}
