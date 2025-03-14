﻿using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Transaction.CreateTransaction;

namespace SnowFlake.Services;

public interface ITransactionService
{
    Task<TransactionEntity> CreateTransaction(TransactionEntity transaction);
    Task<List<TransactionEntity>> GetTransactions(int roundNumber, string shopId);
    Task<List<TransactionEntity>> GetTeamTransactionsByRound(int roundNumber, string shopId, string teamId);
    Task<List<TransactionEntity>> GetTransactionsByTeamId(string teamId);
    Task<List<TransactionEntity>> GetImageTransactions(int roundNumber, string shopId);
    Task<List<TransactionEntity>> GetProductTransactions(int roundNumber, string shopId);
}