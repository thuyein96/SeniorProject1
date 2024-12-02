using MongoDB.Bson;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Team.CreateTeam;
using SnowFlake.Dtos.APIs.Team.UpdateTeam;
using SnowFlake.UnitOfWork;

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
                MaxMembers = createTeamRequest.MaxMembers,
                Tokens = createTeamRequest.Tokens,
                PlaygroundId = createTeamRequest.PlaygroundId,
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
        // TODO: change dynamic for bucket size
        var teams = (await _unitOfWork.TeamRepository.GetAll()).Take(50).ToList();
        return teams;
    }

    public async Task<TeamEntity?> GetById(string TeamId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(TeamId)) return null;

            return (await _unitOfWork.TeamRepository.GetBy(t => t.Id == TeamId)).FirstOrDefault();
        }
        catch (Exception)
        {
            return null;
        }

    }

    public async Task<string> Update(UpdateTeamRequest updateTeamRequest)
    {
        try
        {
            if (updateTeamRequest is null) return string.Empty;

            var existingTeam = (await _unitOfWork.TeamRepository.GetBy(w => w.Id == updateTeamRequest.Id)).SingleOrDefault();

            if (existingTeam is null || existingTeam.Id != updateTeamRequest.Id) return string.Empty;

            existingTeam.TeamNumber = updateTeamRequest.TeamNumber;
            existingTeam.MaxMembers = updateTeamRequest.MaxMembers;
            existingTeam.Tokens = updateTeamRequest.Tokens;
            existingTeam.ModifiedDate = DateTime.Now;

            _unitOfWork.TeamRepository.Update(existingTeam);
            _unitOfWork.Commit();

            return $"[ID: {existingTeam.Id}] Successfully Updated";
        }
        catch (Exception e)
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