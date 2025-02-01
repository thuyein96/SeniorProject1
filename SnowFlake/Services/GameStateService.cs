using MongoDB.Bson;
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

        if (createGameStateRequest.CurrentGameState != GameState.TeamCreation.Value) return null;
        var gameStateEntity = new GameStateEntity
        {
            Id = ObjectId.GenerateNewId().ToString(),
            HostRoomCode = createGameStateRequest.HostRoomCode,
            PlayerRoomCode = createGameStateRequest.PlayerRoomCode,
            CurrentGameState = createGameStateRequest.CurrentGameState,
            CurrentRoundNumber = createGameStateRequest.CurrentRoundNumber,
            CreationDate = DateTime.Now
        };

        await _unitOfWork.GameStateRepository.Create(gameStateEntity);
        await _unitOfWork.Commit();

        return gameStateEntity;
    }



    public async Task<GameStateEntity> GetGameState(GetGameStateRequest getGameStateRequest)
    {
        try
        {
            var gameStateEntity = new GameStateEntity();
            if (!string.IsNullOrWhiteSpace(getGameStateRequest.PlayerRoomCode))
                gameStateEntity = (await _unitOfWork.GameStateRepository.GetBy(g => g.PlayerRoomCode == getGameStateRequest.PlayerRoomCode)).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(getGameStateRequest.HostRoomCode))
                gameStateEntity = (await _unitOfWork.GameStateRepository.GetBy(g => g.HostRoomCode == getGameStateRequest.HostRoomCode)).FirstOrDefault();

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
            var existingGameState = (await _unitOfWork.GameStateRepository.GetBy(g => g.HostRoomCode == updateGameStateRequest.HostRoomCode)).FirstOrDefault();

            if (existingGameState == null) return string.Empty;

            existingGameState.CurrentGameState = updateGameStateRequest.CurrentGameState;
            if(updateGameStateRequest.CurrentRoundNumber is not null)
                existingGameState.CurrentRoundNumber = updateGameStateRequest.CurrentRoundNumber;
            existingGameState.ModifiedDate = DateTime.Now;

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
