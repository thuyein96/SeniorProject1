using SnowFlake.Repositories.Domain;
using SnowFlake.Repository;

namespace SnowFlake.UnitOfWork;

public interface IUnitOfWork
{
    IPlayerRepository PlayerRepository { get; }
    ITeamRepository TeamRepository { get; }
    IPlaygroundRepository PlaygroundRepository { get; } 
    
    //Commit stages (insert, update, delete)
    void Commit();
    //Rollback transaction
    void Rollback();
}