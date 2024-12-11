using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.GameState.CreateGameState;
using SnowFlake.Dtos.APIs.GameState.GetGameState;
using SnowFlake.Dtos.APIs.GameState.UpdateGameState;
using SnowFlake.UnitOfWork;
using SnowFlake.Utilities;

namespace SnowFlake.Services;

public class GameStateService : IGameStateService
{
    private readonly IUnitOfWork _unitOfWork;

    public GameStateService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<GameStateEntity> AddGameState(CreateGameStateRequest createGameStateRequest)
    {
        if (createGameStateRequest == null) return null;
        var gameStateEntity = new GameStateEntity
        {
            HostRoomCode = createGameStateRequest.HostRoomCode,
            PlayerRoomCode = createGameStateRequest.PlayerRoomCode,
            CurrentState = string.Empty
        };

        _unitOfWork.GameStateRepository.Create(gameStateEntity);
        _unitOfWork.Commit();

        return gameStateEntity;
    }



    public async Task<GameStateEntity> GetGameState(string gameStateId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(gameStateId)) return null;
            var gameStateEntity = (await _unitOfWork.GameStateRepository.GetBy(g => g.Id == gameStateId)).FirstOrDefault();
            return gameStateEntity;
        }
        catch (Exception)
        {
            return null;
        }
        
    }

    public async Task<string> UpdateGameState(UpdateGameStateRequest updateGameStateRequest)
    {
        try
        {
            var existingGameState = (await _unitOfWork.GameStateRepository.GetBy(g => g.Id == updateGameStateRequest.Id)).FirstOrDefault();

            if (existingGameState == null) return string.Empty;

            existingGameState.CurrentState = updateGameStateRequest.CurrentState;

            await _unitOfWork.GameStateRepository.Update(existingGameState);
            await _unitOfWork.Commit();

            return $"[ID: {existingGameState.Id}] Successfully Updated";
        }
        catch (Exception)
        {
            return string.Empty;
        }
        
    }
}
