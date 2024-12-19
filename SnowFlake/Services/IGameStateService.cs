using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.GameState.CreateGameState;
using SnowFlake.Dtos.APIs.GameState.GetGameState;
using SnowFlake.Dtos.APIs.GameState.UpdateGameState;

namespace SnowFlake.Services;

public interface IGameStateService
{
    Task<GameStateEntity> AddGameState(CreateGameStateRequest createGameStateRequest);
    Task<string> UpdateGameState(UpdateGameStateRequest updateGameStateRequest);
    Task<GameStateEntity> GetGameState(GetGameStateRequest getGameStateRequest);
}
