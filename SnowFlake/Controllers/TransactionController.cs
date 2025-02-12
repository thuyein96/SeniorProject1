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
    public async Task<IActionResult> GetTransactions([FromQuery] string? hostRoomCode, [FromQuery] string? playerRoomCode, [FromQuery] int roundNumber)
    {
        try
        {
            var transactions = await _transactionManager.GetTransactionsWithShop(hostRoomCode, playerRoomCode, roundNumber);
            if (transactions == null)
            {
                return NotFound(new 
                {
                    Success = false,
                    Message = new List<TransactionEntity>()
                });
            }
            return Ok(new GetTransactionsResponse
            {
                Success = true,
                Message = transactions
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}