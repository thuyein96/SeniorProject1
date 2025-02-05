using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Player;
using SnowFlake.Dtos.APIs.Player.AddPlayerToTeam;
using SnowFlake.Dtos.APIs.Player.SearchPlayer;
using PlayerItem = SnowFlake.Dtos.APIs.Player.PlayerItem;

namespace SnowFlake.Managers;

public interface IPlayerManager
{
    Task<PlayerItem> SearchPlayer(SearchPlayerRequest searchPlayerRequest);
    Task<string> ManagePlayer(ManagePlayerRequest managePlayerRequest);

    //Task<string> RemovePlayerFromTeam(string playerName, int teamNumber, string playerRoomCode);
}
