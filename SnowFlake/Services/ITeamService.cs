using MongoDB.Bson;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Team.CreateTeam;
using SnowFlake.Dtos.APIs.Team.GetTeam;
using SnowFlake.Dtos.APIs.Team.GetTeams;
using SnowFlake.Dtos.APIs.Team.UpdateTeam;

namespace SnowFlake.Services;

public interface ITeamService
{
    //Create 
    TeamEntity Create(CreateTeamRequest createTeamRequest);
    //Reterive
    List<TeamEntity> GetAll();
    //GetById
    TeamEntity GetById(string TeamId);
    //Update 
    string Update(UpdateTeamRequest updateTeamRequest);
    //delete 
    string Delete(string TeamId);
}