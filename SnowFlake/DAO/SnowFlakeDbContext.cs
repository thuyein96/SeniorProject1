using Microsoft.EntityFrameworkCore;

namespace SnowFlake.DAO;

public class SnowFlakeDbContext : DbContext
{
    public SnowFlakeDbContext(DbContextOptions<SnowFlakeDbContext> options) : base(options)
    {

    }

    public DbSet<Dtos.PlayerItem> Players { get; set; }
    public DbSet<Dtos.TeamEntity> Teams { get; set; }
    public DbSet<Dtos.PlaygroundEntity> Playgrounds { get; set; }
    public DbSet<Dtos.ImageEntity> Images { get; set; }
    public DbSet<Dtos.GameStateEntity> GameStates { get; set; }
    public DbSet<Dtos.ShopEntity> Shops { get; set; }
    public DbSet<Dtos.ProductEntity> Products { get; set; }
    public DbSet<Dtos.LeaderboardEntity> Leaderboards { get; set; }
    public DbSet<Dtos.TransactionEntity> Transactions { get; set; }


}