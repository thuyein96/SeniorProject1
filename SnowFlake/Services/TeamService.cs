using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Bson;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Team.CreateTeam;
using SnowFlake.Dtos.APIs.Team.GetTeam;
using SnowFlake.Dtos.APIs.Team.GetTeams;
using SnowFlake.Dtos.APIs.Team.UpdateTeam;
using SnowFlake.Repository;
using SnowFlake.UnitOfWork;

namespace SnowFlake.Services;

public class TeamService : ITeamService
{
    private readonly IUnitOfWork _unitOfWork;

    public TeamService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public bool Create(CreateTeamRequest createTeamRequest)
    {
        try
        {
            if (createTeamRequest is null) return false;

            var team = new TeamEntity
            {
                Id = createTeamRequest.Id.ToString(),
                TeamNumber = createTeamRequest.TeamNumber,
                MaxMembers = createTeamRequest.MaxMembers,
                Tokens = createTeamRequest.Tokens,
                CreationDate = DateTime.Now,
                ModifiedDate = null
            };

            _unitOfWork.TeamRepository.Create(team);
            _unitOfWork.Commit();
            return true;
        }
        catch (Exception)
        {

            return false;
        }
    }

    public GetTeamsResponse GetAll()
    {
        var response = new GetTeamsResponse
        {
            Teams = new List<GetTeamResponse>()
        };
        response.Teams = _unitOfWork.TeamRepository.GetAll().Take(50).Select( t => new GetTeamResponse
        {
            Id = t.Id,
            TeamNumber = t.TeamNumber,
            MaxMembers = t.MaxMembers,
            Tokens = t.Tokens,
            CreationDate = t.CreationDate,
            ModifiedDate = t.ModifiedDate
        }).ToList();
        return response;
    }

    public GetTeamResponse GetById(string TeamId)
    {
        if(string.IsNullOrWhiteSpace(TeamId)) return null;
        
        return _unitOfWork.TeamRepository.GetBy(t => t.Id == TeamId).Select(t => new GetTeamResponse
        {
            Id = t.Id,
            TeamNumber = t.TeamNumber,
            MaxMembers = t.MaxMembers,
            Tokens = t.Tokens,
            CreationDate = t.CreationDate,
            ModifiedDate = t.ModifiedDate,
        }).FirstOrDefault();
    }

    public void Update(UpdateTeamRequest updateTeamRequest)
    {
        if(updateTeamRequest is null) return;
        
        var team = new TeamEntity
        {
            Id = updateTeamRequest.Id,
            TeamNumber = updateTeamRequest.TeamNumber,
            MaxMembers = updateTeamRequest.MaxMembers,
            Tokens = updateTeamRequest.Tokens,
            CreationDate = updateTeamRequest.CreationDate,
            ModifiedDate = DateTime.Now
        };

        _unitOfWork.TeamRepository.Update(team);
        _unitOfWork.Commit();
    }

    public bool Delete(string TeamId)
    {
        try
        {
            TeamEntity team = _unitOfWork.TeamRepository.GetBy(w => w.Id == TeamId).SingleOrDefault();
            
            if(team is null) return false;
            _unitOfWork.TeamRepository.Delete(team);
            _unitOfWork.Commit();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}