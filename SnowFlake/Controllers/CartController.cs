using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Cart;
using SnowFlake.Dtos.APIs.Cart.GetTeamCartItems;
using SnowFlake.Dtos.APIs.Cart.RemoveCartItem;
using SnowFlake.Managers;

namespace SnowFlake.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartManager _cartManager;

    public CartController(ICartManager cartManager)
    {
        _cartManager = cartManager;
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(AddCartItemRequest addCartItemRequest)
    {
        try
        {
            var cartItems = await _cartManager.AddToCart(addCartItemRequest);

            if (cartItems is null)
                return NotFound(new AddCartItemResponse
                {
                    Success = false,
                    Message = null
                });

            return Ok(new AddCartItemResponse
            {
                Success = true,
                Message = cartItems
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
        
    }

    [HttpGet]
    public async Task<IActionResult> GetTeamCartItems([FromQuery] string? hostRoomCode,
                                                      [FromQuery] string? playerRoomCode, 
                                                      [FromQuery] int teamNumber)
    {
        try
        {
            var teamCartItems = await _cartManager.GetCartItemsByRoomCode(hostRoomCode, playerRoomCode, teamNumber);

            if (teamCartItems is null)
            {
                return NotFound(new GetTeamCartItemsResponse
                {
                    Success = false,
                    Message = null
                });
            }

            return Ok(new GetTeamCartItemsResponse
            {
                Success = true,
                Message = teamCartItems
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveCartItem(RemoveCartItemRequest removeCartItemRequest)
    {
        try
        {
            if (removeCartItemRequest is null)
                return BadRequest(new RemoveCartItemResponse
                {
                    Success = false,
                    Message = string.Empty
                });

            var removeCartItem = await _cartManager.RemoveCart(removeCartItemRequest);

            if (removeCartItem is null)
                return NotFound(new RemoveCartItemResponse
                {
                    Success = false,
                    Message = string.Empty
                });

            return Ok(new RemoveCartItemResponse
            {
                Success = true,
                Message = removeCartItem
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }
}