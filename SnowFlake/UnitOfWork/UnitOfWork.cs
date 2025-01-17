using SnowFlake.DAO;
using SnowFlake.Repositories.Domain;
using SnowFlake.Repository;

namespace SnowFlake.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly SnowFlakeDbContext _snowFlakeDbContext;

    public UnitOfWork(SnowFlakeDbContext snowFlakeDbContext)
    {
        _snowFlakeDbContext = snowFlakeDbContext;
    }

    private IPlayerRepository _playerRepository;
    public IPlayerRepository PlayerRepository
    {
        get
        {
            return _playerRepository = _playerRepository ?? new PlayerRepository(_snowFlakeDbContext); 
        }
    }
    
    private ITeamRepository _teamRepository;

    public ITeamRepository TeamRepository
    {
        get
        {
            return _teamRepository = _teamRepository ?? new TeamRepository(_snowFlakeDbContext);
        }
    }

    private IPlaygroundRepository _playgroundRepository;

    public IPlaygroundRepository PlaygroundRepository
    {
        get
        {
            return _playgroundRepository = _playgroundRepository ?? new PlaygroundRepository(_snowFlakeDbContext);
        }
    }

    private IImageRepository _imageRepository;

    public IImageRepository ImageRepository
    {
        get
        {
            return _imageRepository = _imageRepository ?? new ImageRepository(_snowFlakeDbContext);
        }
    }

    private IRoundRepository _roundRepository;

    public IRoundRepository RoundRepository
    {
        get
        {
            return _roundRepository = _roundRepository ?? new RoundRepository(_snowFlakeDbContext);
        }
    }

    private IGameStateRepository _gameStateRepository;
    public IGameStateRepository GameStateRepository
    {
        get
        {
            return _gameStateRepository = _gameStateRepository ?? new GameStateRepository(_snowFlakeDbContext);
        }
    }

    private IShopRepository _shopRepository;
    public IShopRepository ShopRepository
    {
        get
        {
            return _shopRepository = _shopRepository ?? new ShopRepository(_snowFlakeDbContext);
        }
    }

    private ILeaderboardRepository _leaderboardRepository;

    public ILeaderboardRepository LeaderboardRepository
    {
        get
        {
            return _leaderboardRepository = _leaderboardRepository ?? new LeaderboardRepository(_snowFlakeDbContext);
        }
    }

    public async Task Commit()
    {
        _snowFlakeDbContext.SaveChanges();
    }

    public async Task Rollback()
    {
        _snowFlakeDbContext.Dispose();
    }
}