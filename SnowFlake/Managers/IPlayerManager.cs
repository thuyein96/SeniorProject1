using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Player;
using SnowFlake.Dtos.APIs.Player.AddPlayerToTeam;

namespace SnowFlake.Managers;

public interface IPlayerManager
{
    Task<List<PlayerItem>> SearchPlayersByTeamNumber(int teamNumber, string playerRoomCode);
    Task<string> ManagePlayer(ManagePlayerRequest managePlayerRequest);

    //Task<string> RemovePlayerFromTeam(string playerName, int teamNumber, string playerRoomCode);
}
