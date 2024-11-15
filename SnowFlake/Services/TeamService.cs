﻿using SnowFlake.Dtos;
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
    public void Create(CreateTeamRequest createTeamRequest)
    {
        if (createTeamRequest is null) return;
        var team = new TeamEntity
        {
            Id = Guid.NewGuid().ToString(),
            TeamNumber = createTeamRequest.TeamNumber,
            ProfileImageUrl = createTeamRequest.ProfileImageUrl,
            CreationDate = DateTime.Now,
        };
        
        _unitOfWork.TeamRepository.Create(team);
        _unitOfWork.Commit();
    }

    public GetTeamsResponse GetAll()
    {
        GetTeamsResponse response = new GetTeamsResponse
        {
            Teams = new List<TeamEntity>()
        };
        response.Teams = _unitOfWork.TeamRepository.GetAll().Select(t => new TeamEntity
        {
            Id = t.Id.ToString(),
            TeamNumber = t.TeamNumber,
            Tokens = t.Tokens,
            CreationDate = t.CreationDate,
            ModifiedDate = t.ModifiedDate
        }).ToList();
        return response;
    }

    public GetTeamResponse GetById(string TeamId)
    {
        if(string.IsNullOrWhiteSpace(TeamId)) return null;
        
        return _unitOfWork.TeamRepository.GetBy(t => t.Id == TeamId).Select(s => new GetTeamResponse
        {
            Id = s.Id,
            Tokens = s.Tokens,
            MaxMembers = s.MaxMembers,
            ProfileImageUrl = s.ProfileImageUrl,
            TeamNumber = s.TeamNumber,
            CreatedOn = s.CreationDate,
            ModifiedOn = s.ModifiedDate
        }).FirstOrDefault();
    }

    public void Update(UpdateTeamRequest updateTeamRequest)
    {
        if(updateTeamRequest is null) return;

        var team = new TeamEntity
        {
            Id = updateTeamRequest.Id,
            TeamNumber = updateTeamRequest.TeamNumber,
            Tokens = updateTeamRequest.Tokens,
            MaxMembers = updateTeamRequest.MaxMembers,
            ProfileImageUrl = updateTeamRequest.ProfileImageUrl,
            CreationDate = updateTeamRequest.CreatedOn,
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