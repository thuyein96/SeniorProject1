﻿using MongoDB.Bson;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Product;
using SnowFlake.Dtos.APIs.Team.CreateTeam;
using SnowFlake.Dtos.APIs.Team.GetTeamsByRoomCode;
using SnowFlake.Dtos.APIs.Team.SearchPlayerInTeam;
using SnowFlake.Dtos.APIs.Team.UpdateTeam;
using SnowFlake.UnitOfWork;
using SnowFlake.Utilities;

namespace SnowFlake.Services;

public class TeamService : ITeamService
{
    private readonly IUnitOfWork _unitOfWork;

    public TeamService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<TeamEntity> Create(CreateTeamRequest createTeamRequest)
    {
        try
        {
            if (createTeamRequest is null) return null;

            var team = new TeamEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                TeamNumber = createTeamRequest.TeamNumber,
                Tokens = createTeamRequest.Tokens,
                HostRoomCode = createTeamRequest.HostRoomCode,
                PlayerRoomCode = createTeamRequest.PlayerRoomCode,
                CreationDate = DateTime.Now,
                ModifiedDate = null
            };

            _unitOfWork.TeamRepository.Create(team);
            _unitOfWork.Commit();
            return team;
        }
        catch (Exception)
        {

            return null;
        }
    }

    public async Task<List<TeamEntity>> GetAll()
    {
        var teams = (await _unitOfWork.TeamRepository.GetAll()).Take(Utils.BatchSize).ToList();
        return teams;
    }
    public async Task<TeamEntity> GetTeam(int teamNumber, string? playerRoomCode, string? hostRoomCode)
    {
        try
        {
            if (teamNumber <= 0) return null;

            if (!string.IsNullOrWhiteSpace(playerRoomCode))
                return (await _unitOfWork.TeamRepository.GetBy(t => t.TeamNumber == teamNumber && t.PlayerRoomCode == playerRoomCode)).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(hostRoomCode))
                return (await _unitOfWork.TeamRepository.GetBy(t => t.TeamNumber == teamNumber && t.HostRoomCode == hostRoomCode)).FirstOrDefault();

            return null;
        }
        catch (Exception)
        {
            return null;
        }

    }

    public async Task<List<TeamEntity>> GetTeamsByRoomCode(GetTeamsByRoomCodeRequest getTeamsByRoomCodeRequest)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(getTeamsByRoomCodeRequest.HostRoomCode))
                return (await _unitOfWork.TeamRepository.GetBy(t => t.HostRoomCode == getTeamsByRoomCodeRequest.HostRoomCode)).ToList();
            if (!string.IsNullOrWhiteSpace(getTeamsByRoomCodeRequest.PlayerRoomCode))
                return (await _unitOfWork.TeamRepository.GetBy(t => t.PlayerRoomCode == getTeamsByRoomCodeRequest.PlayerRoomCode)).ToList();
            return null;
        }
        catch (Exception)
        {
            return null;
        }

    }


    public async Task<bool> MinusTeamTokens(TeamEntity team, int totalCost)
    {
        try
        {
            if(team is null || totalCost <= 0) return false;

            team.Tokens -= totalCost;
            team.ModifiedDate = DateTime.Now;

            _unitOfWork.TeamRepository.Update(team);
            _unitOfWork.Commit();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<string> Update(UpdateTeamRequest updateTeamRequest)
    {
        try
        {
            if (updateTeamRequest is null) return string.Empty;

            var existingTeam = (await _unitOfWork.TeamRepository.GetBy(w => w.Id == updateTeamRequest.Id)).SingleOrDefault();

            if (existingTeam is null || existingTeam.Id != updateTeamRequest.Id) return string.Empty;
            if (updateTeamRequest.Tokens is not null)
                existingTeam.Tokens = updateTeamRequest.Tokens;
            //if (updateTeamRequest.Member is not null && existingTeam.Members is not null)
            //    existingTeam.Members.Add(updateTeamRequest.Member);
            //if (updateTeamRequest.Member is not null && existingTeam.Members is null)
            //{
            //    existingTeam.Members = new List<string>();
            //    existingTeam.Members.Add(updateTeamRequest.Member);
            //}
            existingTeam.ModifiedDate = DateTime.Now;

            _unitOfWork.TeamRepository.Update(existingTeam);
            _unitOfWork.Commit();

            return $"[ID: {existingTeam.Id}] Successfully Updated";
        }
        catch (Exception)
        {
            return string.Empty;
        }

    }

    public async Task<string> Delete(string TeamId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(TeamId)) return string.Empty;

            var team = (await _unitOfWork.TeamRepository.GetBy(w => w.Id == TeamId)).SingleOrDefault();
            if (team is null) return string.Empty;

            _unitOfWork.TeamRepository.Delete(team);
            _unitOfWork.Commit();
            return $"[ID: {team.Id}] Successfully Deleted";
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }
}