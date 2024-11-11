using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Team.CreateTeam;
using SnowFlake.Dtos.APIs.Team.GetTeam;
using SnowFlake.Dtos.APIs.Team.GetTeams;
using SnowFlake.Dtos.APIs.Team.UpdateTeam;

namespace SnowFlake.Services;

public interface ITeamService
{
    //Create 
    void Create(CreateTeamRequest createTeamRequest);
    //Reterive
    GetTeamsResponse GetAll();
    //GetById
    GetTeamResponse GetById(string TeamId);
    //Update 
    void Update(UpdateTeamRequest updateTeamRequest);
    //delete 
    bool Delete(string TeamId);
}