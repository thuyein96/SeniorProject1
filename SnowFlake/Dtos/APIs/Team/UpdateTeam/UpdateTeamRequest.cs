namespace SnowFlake.Dtos.APIs.Team.UpdateTeam;

public class UpdateTeamRequest
{
    public string Id { get; set; }
    public int TeamNumber { get; set; }
    public int MaxMembers { get; set; }
    public int Tokens { get; set; }
    public List<(Stream ImageByteData, string BuyingStatus)>? SnowFlakeImageUrls { get; set; }
    public string PlaygroundId { get; set; }
}