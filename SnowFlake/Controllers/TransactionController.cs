using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Transaction.GetTransactions;
using SnowFlake.Managers;
using SnowFlake.Services;

namespace SnowFlake.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly ITransactionManager _transactionManager;

    public TransactionController(ITransactionManager transactionManager)
    {
        _transactionManager = transactionManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactions([FromQuery] string? hostRoomCode, [FromQuery] string? playerRoomCode, [FromQuery] int roundNumber, [FromQuery] int? teamNumber)
    {
        try
        {
            var transactions = await _transactionManager.GetTransactionsWithShop(hostRoomCode, playerRoomCode, roundNumber, teamNumber);
            
            return transactions.Success ? Ok(transactions) : NotFound(transactions);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("itemtransactions")]
    public async Task<IActionResult> GetItemTransactions([FromQuery] string? hostRoomCode, [FromQuery] string? playerRoomCode, [FromQuery] int roundNumber, [FromQuery] int? teamNumber)
    {
        try
        {
            var transactions = await _transactionManager.GetItemTransactionsWithShop(hostRoomCode, playerRoomCode, roundNumber, teamNumber);

            return transactions.Success ? Ok(transactions) : NotFound(transactions);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("imagetransactions")]
    public async Task<IActionResult> GetImageTransactions([FromQuery] string? hostRoomCode, [FromQuery] string? playerRoomCode, [FromQuery] int roundNumber, [FromQuery] int? teamNumber)
    {
        try
        {
            var transactions = await _transactionManager.GetImageTransactionsWithShop(hostRoomCode, playerRoomCode, roundNumber, teamNumber);

            return transactions.Success ? Ok(transactions) : NotFound(transactions);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}