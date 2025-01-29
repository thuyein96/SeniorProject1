using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Player;
using SnowFlake.Dtos.APIs.Player.AddPlayerToTeam;
using SnowFlake.Dtos.APIs.Player.SearchPlayer;
using SnowFlake.Dtos.APIs.Player.UpdatePlayer;
using SnowFlake.Services;

namespace SnowFlake.Managers;

public class PlayerManager : IPlayerManager
{
    private readonly IPlayerService _playerService;
    private readonly ITeamService _teamService;

    public PlayerManager(IPlayerService playerService,
                       ITeamService teamService)
    {
        _playerService = playerService;
        _teamService = teamService;
    }

    public async Task<PlayerItem> SearchPlayer(SearchPlayerRequest searchPlayerRequest)
    {
        var player = new PlayerEntity();

        if (searchPlayerRequest.TeamNumber == null || searchPlayerRequest.TeamNumber <= 0)
        {
            player = await _playerService.GetPlayerByRoomCode(searchPlayerRequest.PlayerName, searchPlayerRequest.PlayerRoomCode);

            return new PlayerItem
            {
                Id = player.Id,
                PlayerName = player.Name,
                PlayerRoomCode = player.RoomCode,
                TeamId = player.TeamId
            };
        }

        var team = await _teamService.GetTeam(searchPlayerRequest.TeamNumber.Value, searchPlayerRequest.PlayerRoomCode, null);
        if (team is null)
        {
            return null;
        }

        player = await _playerService.GetPlayerByName(searchPlayerRequest.PlayerName, team.Id, searchPlayerRequest.PlayerRoomCode);

        return new PlayerItem
        {
            Id = player.Id,
            PlayerName = player.Name,
            PlayerRoomCode = player.RoomCode,
            TeamId = player.TeamId,
            TeamNumber = team.TeamNumber
        };
    }
    public async Task<string> ManagePlayer(ManagePlayerRequest managePlayerRequest)
    {
        try
        {
            var team = await _teamService.GetTeam(managePlayerRequest.TeamNumber, managePlayerRequest.PlayerRoomCode, null);
            if (team is null)
            {
                return null;
            }

            var updatedPlayer = string.Empty;
            var player = new PlayerEntity();

            if (managePlayerRequest.Status.ToLower() == "add")
            {
                player = await _playerService.GetPlayerByName(managePlayerRequest.PlayerName);
                if (player is null) return null;

                if (!string.IsNullOrWhiteSpace(player.TeamId)) return null;

                updatedPlayer = await _playerService.Update(new UpdatePlayerRequest
                {
                    Id = player.Id,
                    PlayerName = player.Name,
                    RoomCode = managePlayerRequest.PlayerRoomCode,
                    TeamId = team.Id
                });
            }
            else if (managePlayerRequest.Status.ToLower() == "remove")
            {
                player = await _playerService.GetPlayerByName(managePlayerRequest.PlayerName, team.Id, managePlayerRequest.PlayerRoomCode);
                if (player is null) return null;

                updatedPlayer = await _playerService.Update(new UpdatePlayerRequest
                {
                    Id = player.Id,
                    PlayerName = player.Name,
                    RoomCode = player.RoomCode,
                    TeamId = null
                });
            }

            if (string.IsNullOrWhiteSpace(updatedPlayer)) return null;
            return $"[Team: {managePlayerRequest.TeamNumber}][Player: {managePlayerRequest.PlayerName}] Successfully {managePlayerRequest.Status}.";
        }
        catch (Exception e)
        {
            return null;
        }
    }
}
