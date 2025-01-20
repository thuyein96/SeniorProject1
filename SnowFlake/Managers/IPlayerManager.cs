using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Player;
using SnowFlake.Dtos.APIs.Player.AddPlayerToTeam;
using SnowFlake.Dtos.APIs.Player.SearchPlayer;

namespace SnowFlake.Managers;

public interface IPlayerManager
{
    Task<PlayerItem> SearchPlayer(SearchPlayerRequest searchPlayerRequest);
    Task<string> ManagePlayer(ManagePlayerRequest managePlayerRequest);

    //Task<string> RemovePlayerFromTeam(string playerName, int teamNumber, string playerRoomCode);
}
