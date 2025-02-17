using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Leaderboard;
using SnowFlake.Dtos.APIs.Leaderboard.CreateLeaderboard;
using SnowFlake.Dtos.APIs.Leaderboard.GetLeaderboard;
using SnowFlake.Dtos.APIs.Team.GetTeamsByRoomCode;
using SnowFlake.Services;

namespace SnowFlake.Managers;

public class LeaderboardManager : ILeaderboardManager
{
    private readonly IShopService _shopService;
    private readonly IImageService _imageService;
    private readonly ILeaderboardService _leaderboardService;
    private readonly ITeamService _teamService;
    private readonly IPlayerService _playerService;
    private readonly IProductService _productService;
    private readonly ITransactionService _transactionService;

    public LeaderboardManager(IShopService shopService,
        IImageService imageService,
        ILeaderboardService leaderboardService,
        ITeamService teamService,
        IPlayerService playerService,
        IProductService productService,
        ITransactionService transactionService)
    {
        _shopService = shopService;
        _imageService = imageService;
        _leaderboardService = leaderboardService;
        _teamService = teamService;
        _playerService = playerService;
        _productService = productService;
        _transactionService = transactionService;
    }

    public async Task<CreateLeaderboardResponse> CreateLeaderboard(string hostRoomCode, string playerRoomCode)
    {
        var teamDetailsList = new List<TeamRankDetails>();
        var teams = new List<TeamEntity>();
        var shop = new ShopEntity();

        if (!string.IsNullOrWhiteSpace(hostRoomCode))
        {
            teams = await _teamService.GetTeamsByRoomCode(new GetTeamsByRoomCodeRequest
            {
                HostRoomCode = hostRoomCode
            });

            shop = await _shopService.GetShopByHostRoomCode(hostRoomCode);
        }
        if(!string.IsNullOrWhiteSpace(playerRoomCode))
        {
            teams = await _teamService.GetTeamsByRoomCode(new GetTeamsByRoomCodeRequest
            {
                PlayerRoomCode = playerRoomCode
            });
            shop = await _shopService.GetShopByPlayerRoomCode(playerRoomCode);
        }

        if (shop is null) return new CreateLeaderboardResponse
        {
            Success = false,
            Message = null
        };

        foreach (var team in teams)
        {
            var teamDetails = new TeamRankDetails();
            teamDetails.TeamNumber = team.TeamNumber;
            teamDetails.RemainingTokens = (int)team.Tokens;

            teamDetails.Players = new List<string>();
            var teamPlayers = await _playerService.GetPlayersByTeamId(team.Id);
            teamDetails.Players = teamPlayers.Select(p => p.PlayerName).ToList();

            teamDetails.Stocks = new List<ProductEntity>();
            teamDetails.Stocks = await _productService.GetProductsByOwnerId(team.Id);

            var transactions = await _transactionService.GetTransactionsByTeamId(team.Id);
            var totalSales = transactions.Select(t => t.Total).Sum();
            teamDetails.TotalSales = totalSales;

            teamDetailsList.Add(teamDetails);
        }

        // Calculate rank based on total sales
        var rankedTeams = await TagTeamRankNumber(teamDetailsList);

        var leaderboard = (await _leaderboardService.CreateLeaderboard(new CreateLeaderboardRequest
        {
            HostRoomCode = shop.HostRoomCode,
            PlayerRoomCode = shop.PlayerRoomCode,
            TeamRanks = rankedTeams.Select(t => new TeamRank
            {
                Id = ObjectId.GenerateNewId().ToString(),
                TeamNumber = t.TeamNumber,
                Rank = t.TeamRank,
                RemainingTokens = t.RemainingTokens,
                TotalSnowFlakeSales = t.TotalSales,
                CreationDate = DateTime.Now,
                ModifiedDate = null
            }).ToList()
        }));

        return rankedTeams is null || rankedTeams.Count <= 0 
            ? new CreateLeaderboardResponse
            {
                Success = false,
                Message = null
            }
            :new CreateLeaderboardResponse
            {
                Success = true,
                Message = teamDetailsList
            };
    }

    private async Task<List<TeamRankDetails>> TagTeamRankNumber(List<TeamRankDetails> teamDetailsList)
    {
        var rankedTeams = teamDetailsList.OrderByDescending(t => t.TotalSales).ToList();
        for (int i = 0; i < rankedTeams.Count; i++)
        {
            rankedTeams[i].TeamRank = i + 1;
        }

        return rankedTeams;
    }

    public async Task<GetLeaderboardResponse> GetLeaderboard(string? hostRoomCode, string? playerRoomCode)
    {
        try
        {
            var leaderboard = new LeaderboardEntity();
            if(string.IsNullOrWhiteSpace(hostRoomCode))
            {
                leaderboard = await _leaderboardService.GetLeaderboardByHostRoomCode(hostRoomCode);
            };
            if (string.IsNullOrWhiteSpace(playerRoomCode))
            {
                leaderboard = await _leaderboardService.GetLeaderboardByPlayerRoomCode(playerRoomCode);
            };

            if (leaderboard.TeamRanks.Count <= 0) return new GetLeaderboardResponse
            {
                Success = false,
                Message = null
            };

            var teamDetailsList = new List<TeamRankDetails>();
            foreach (var teamRank in leaderboard.TeamRanks)
            {
                var teamDetails = new TeamRankDetails();
                var team = await _teamService.GetTeam(teamRank.TeamNumber, null, hostRoomCode);

                teamDetails.TeamNumber = team.TeamNumber;
                teamDetails.TeamRank = teamRank.Rank;
                teamDetails.RemainingTokens = teamRank.RemainingTokens;
                teamDetails.TotalSales = teamRank.TotalSnowFlakeSales;

                teamDetails.Players = new List<string>();
                var teamPlayers = await _playerService.GetPlayersByTeamId(team.Id);
                teamDetails.Players = teamPlayers.Select(p => p.PlayerName).ToList();

                teamDetails.Stocks = new List<ProductEntity>();
                teamDetails.Stocks = await _productService.GetProductsByOwnerId(team.Id);
                    
                teamDetailsList.Add(teamDetails);
            }
            return teamDetailsList is null || teamDetailsList.Count == 0 
                ? new GetLeaderboardResponse
                {
                    Success = false,
                    Message = null
                } : new GetLeaderboardResponse
                {
                    Success = true,
                    Message = teamDetailsList
                };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}