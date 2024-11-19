﻿using SnowFlake.DAO;
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

    public void Commit()
    {
        _snowFlakeDbContext.SaveChanges();
    }

    public void Rollback()
    {
        _snowFlakeDbContext.Dispose();
    }
}