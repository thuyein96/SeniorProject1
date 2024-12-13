﻿using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.GameState.CreateGameState;
using SnowFlake.Dtos.APIs.GameState.GetGameState;
using SnowFlake.Dtos.APIs.GameState.UpdateGameState;
using SnowFlake.UnitOfWork;

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
            var existingGameState = (await _unitOfWork.GameStateRepository.GetBy(g => g.Id == updateGameStateRequest.Id)).FirstOrDefault();

            if (existingGameState == null) return string.Empty;

            existingGameState.CurrentState = updateGameStateRequest.CurrentGameState;

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
