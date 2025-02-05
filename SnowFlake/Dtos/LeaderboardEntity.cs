using System.Security.Principal;
using MongoDB.EntityFrameworkCore;

namespace SnowFlake.Dtos;
[Collection("Leaderboard")]
public class LeaderboardEntity : BaseEntity
{
    public string HostRoomCode { get; set; }
    public string PlayerRoomCode { get; set; }
    public List<TeamRank> TeamRanks { get; set; }
}
