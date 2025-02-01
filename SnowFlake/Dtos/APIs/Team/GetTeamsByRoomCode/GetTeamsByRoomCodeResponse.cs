using SnowFlake.Dtos.APIs.Team.GetTeams;

namespace SnowFlake.Dtos.APIs.Team.GetTeamsByRoomCode;

public class GetTeamsByRoomCodeResponse : BaseResponse
{
    public List<TeamWithProducts> Message { get; set; }
}
