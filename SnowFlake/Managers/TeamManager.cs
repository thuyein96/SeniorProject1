using SnowFlake.Services;

namespace SnowFlake.Managers;

public class TeamManager : ITeamManager
{
    private readonly ITeamService _teamService;
    private readonly IProductService _productService;

    public TeamManager(ITeamService teamService,
                       IProductService productService)
    {
        _teamService = teamService;
        _productService = productService;
    }

    public async Task<string> UpdateTeamStock()
    {
        return "Team stock updated.";
    }
}