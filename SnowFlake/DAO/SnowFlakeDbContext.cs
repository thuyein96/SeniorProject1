﻿using Microsoft.EntityFrameworkCore;

namespace SnowFlake.DAO;

public class SnowFlakeDbContext : DbContext
{
    public SnowFlakeDbContext(DbContextOptions<SnowFlakeDbContext> options) : base(options)
    {

    }

    public DbSet<Dtos.PlayerEntity> Players { get; set; }
    public DbSet<Dtos.TeamEntity> Teams { get; set; }
    public DbSet<Dtos.PlaygroundEntity> Playgrounds { get; set; }
    public DbSet<Dtos.ImageEntity> Images { get; set; }
    public DbSet<Dtos.GameStateEntity> GameStates { get; set; }
    public DbSet<Dtos.ShopEntity> Shops { get; set; }


}