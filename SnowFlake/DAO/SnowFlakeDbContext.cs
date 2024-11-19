using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SnowFlake.Dtos;

namespace SnowFlake.DAO;

public class SnowFlakeDbContext : DbContext
{
    public SnowFlakeDbContext(DbContextOptions<SnowFlakeDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Dtos.PlayerEntity> Players { get; set; }
    public DbSet<Dtos.TeamEntity> Teams { get; set; }
    public DbSet<Dtos.PlaygroundEntity> Playgrounds { get; set; }
    
}