using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Team.CreateTeam;
using SnowFlake.Dtos.APIs.Team.GetTeams;

namespace SnowFlake.Services;

public interface ITeamService
{
    //Create 
    void Create(CreateTeamRequest createTeamRequest);
    //Reterive
    GetTeamsResponse GetAll();
    //GetById
    TeamEntity GetById(string TeamId);
    //Update 
    void Update(TeamEntity updateTeam);
    //delete 
    bool Delete(string TeamId);
}