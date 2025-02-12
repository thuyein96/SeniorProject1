using MongoDB.Bson;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Transaction.CreateTransaction;
using SnowFlake.UnitOfWork;
using SnowFlake.Utilities;

namespace SnowFlake.Services;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork _unitOfWork;

    public TransactionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TransactionEntity> CreateTransaction(CreateTransactionRequest createTransactionRequest)
    {
        var transaction = new TransactionEntity
        {
            Id = ObjectId.GenerateNewId().ToString(),
            ShopId = createTransactionRequest.ShopId,
            TeamId = createTransactionRequest.TeamId,
            Quantity = createTransactionRequest.Quantity,
            Total = createTransactionRequest.Total,
            CreationDate = DateTime.Now,
            ModifiedDate = null
        };

        if (!string.IsNullOrWhiteSpace(createTransactionRequest.ImageId))
        {
            transaction.ImageId = createTransactionRequest.ImageId;
        }

        if (!string.IsNullOrWhiteSpace(createTransactionRequest.ProductId))
        {
            transaction.ProductId = createTransactionRequest.ProductId;
        }
        
        await _unitOfWork.TransactionRepository.Create(transaction);
        await _unitOfWork.Commit();

        return transaction;
    }

    public async Task<List<TransactionEntity>> GetTransactions(int roundNumber, string shopId)
    {
        var transactions = (await _unitOfWork.TransactionRepository.GetBy(i => i.ShopId == shopId && i.RoundNumber == roundNumber)).ToList();
        return transactions;
    }

    public async Task<List<TransactionEntity>> GetTransactionsByTeamId(string teamId)
    {
        var transactions = (await _unitOfWork.TransactionRepository.GetBy(i => i.TeamId == teamId)).ToList();
        return transactions;
    }

    public async Task<List<TransactionEntity>> GetImageTransactions(int roundNumber, string shopId)
    {
        var transactions = (await _unitOfWork.TransactionRepository.GetBy(i => i.ImageId != null && i.RoundNumber == roundNumber && i.ShopId == shopId)).ToList();
        return transactions;
    }

    public async Task<List<TransactionEntity>> GetProductTransactions(int roundNumber, string shopId)
    {
        var transactions = (await _unitOfWork.TransactionRepository.GetBy(i => i.ProductId != null & i.RoundNumber == roundNumber && i.ShopId == shopId)).ToList();
        return transactions;
    }


}