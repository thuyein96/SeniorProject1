using SnowFlake.Dtos.APIs.Player.AddPlayerToTeam;

namespace SnowFlake.Managers;

public interface IPlayerManager
{
    Task<string> ManagePlayer(ManagePlayerRequest managePlayerRequest);

    //Task<string> RemovePlayerFromTeam(string playerName, int teamNumber, string playerRoomCode);
}
