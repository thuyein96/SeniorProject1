namespace SnowFlake.Dtos.APIs.Team.CreateTeam;

public class CreateTeamResponse
{
    public Guid Id { get; set; }
    public string TeamNumber { get; set; }
    public int  MaxMemeber { get; set; }
    public int Token { get; set; }
    public string? ProfileImageUrl { get; set; }
}