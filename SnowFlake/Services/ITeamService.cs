using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Team.CreateTeam;
using SnowFlake.Dtos.APIs.Team.UpdateTeam;

namespace SnowFlake.Services;

public interface ITeamService
{
    //Create 
    Task<TeamEntity> Create(CreateTeamRequest createTeamRequest);
    //Reterive
    Task<List<TeamEntity>> GetAll();
    //GetById
    Task<TeamEntity> GetById(string TeamId);
    //Update 
    Task<string> Update(UpdateTeamRequest updateTeamRequest);
    //delete 
    Task<string> Delete(string TeamId);
}