namespace SnowFlake.Dtos.APIs.Team.GetTeam;

public class GetTeamResponse
{
    public Guid Id { get; set; }
    public string TeamNumber { get; set; }
    public int MaxMember { get; set; }
    public int Token { get; set; }
    public string? ProfileImageUrl { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}