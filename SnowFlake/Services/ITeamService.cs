﻿using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Product;
using SnowFlake.Dtos.APIs.Team.CreateTeam;
using SnowFlake.Dtos.APIs.Team.DeleteTeam;
using SnowFlake.Dtos.APIs.Team.GetTeamsByRoomCode;
using SnowFlake.Dtos.APIs.Team.SearchPlayerInTeam;
using SnowFlake.Dtos.APIs.Team.UpdateTeam;

namespace SnowFlake.Services;

public interface ITeamService
{
    //Create 
    Task<TeamEntity> Create(CreateTeamRequest createTeamRequest);
    //Reterive
    Task<List<TeamEntity>> GetAll();
    Task<TeamEntity> GetTeam(int teamNumber, string? playerRoomCode, string? hostRoomCode);
    //GetByRoomCode
    Task<List<TeamEntity>> GetTeamsByRoomCode(GetTeamsByRoomCodeRequest getTeamsByRoomCodeRequest);
    Task<bool> MinusTeamTokens(TeamEntity team, int totalCost);
    //Update 
    Task<string> Update(UpdateTeamRequest updateTeamRequest);
    //delete 
    Task<string> Delete(DeleteTeamRequest deleteTeamRequest);
}