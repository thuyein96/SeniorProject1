namespace SnowFlake.Dtos.APIs.Team.CreateTeam;

public class CreateTeamRequest
{
    public string TeamNumber { get; set; }
    public int MaxMember { get; set; }
    public Dictionary<string, int> Items { get; set; }
    public int Token { get; set; }
    public string ProfileImageUrl { get; set; }
}