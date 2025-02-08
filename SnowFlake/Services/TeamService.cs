using MongoDB.Bson;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Product;
using SnowFlake.Dtos.APIs.Team.CreateTeam;
using SnowFlake.Dtos.APIs.Team.DeleteTeam;
using SnowFlake.Dtos.APIs.Team.GetTeamsByRoomCode;
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
            if (updateTeamRequest.Tokens <= 0) return string.Empty;

            var existingTeam = new TeamEntity();
            if (!string.IsNullOrWhiteSpace(updateTeamRequest.HostRoomCode))
            {
                existingTeam = (await _unitOfWork.TeamRepository.GetBy(w => w.TeamNumber == updateTeamRequest.TeamNumber && w.HostRoomCode == updateTeamRequest.HostRoomCode)).FirstOrDefault();
            }

            if (!string.IsNullOrWhiteSpace(updateTeamRequest.PlayerRoomCode))
            {
                existingTeam = (await _unitOfWork.TeamRepository.GetBy(w => w.TeamNumber == updateTeamRequest.TeamNumber && w.PlayerRoomCode == updateTeamRequest.PlayerRoomCode)).FirstOrDefault();
            }
            if (existingTeam is null) return string.Empty;

            
            existingTeam.Tokens = updateTeamRequest.Tokens;
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

    public async Task<string> Delete(DeleteTeamRequest deleteTeamRequest)
    {
        try
        {
            if (deleteTeamRequest.TeamNumber <= 0) return string.Empty;

            var team = new TeamEntity();
            if (!string.IsNullOrWhiteSpace(deleteTeamRequest.HostRoomCode))
            {
                team = (await _unitOfWork.TeamRepository.GetBy(w => w.TeamNumber == deleteTeamRequest.TeamNumber && w.HostRoomCode == deleteTeamRequest.HostRoomCode)).FirstOrDefault();
            }

            if (!string.IsNullOrWhiteSpace(deleteTeamRequest.PlayerRoomCode))
            {
                team = (await _unitOfWork.TeamRepository.GetBy(w => w.TeamNumber == deleteTeamRequest.TeamNumber && w.PlayerRoomCode == deleteTeamRequest.PlayerRoomCode)).FirstOrDefault();
            }
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