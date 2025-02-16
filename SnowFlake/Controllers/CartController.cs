using Microsoft.AspNetCore.Mvc;
using SnowFlake.Dtos.APIs.Cart;
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

            return cartItems.Success ? Ok(cartItems) : BadRequest(cartItems);
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
            if (teamNumber <= 0)
                return BadRequest("Require team number.");

            var teamCartItems = await _cartManager.GetCartItemsByRoomCode(hostRoomCode, playerRoomCode, teamNumber);

            return teamCartItems.Success ? Ok(teamCartItems) : NotFound(teamCartItems);
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }

    [HttpDelete("{cartId}")]
    public async Task<IActionResult> RemoveCartItem(string cartId)
    {
        try
        {
            if (cartId is null)
                return BadRequest(new RemoveCartItemResponse
                {
                    Success = false,
                    Message = "Cart ID is required."
                });

            var removeCartItem = await _cartManager.RemoveCart(cartId);

            return removeCartItem.Success ? Ok(removeCartItem) : NotFound(removeCartItem);
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }
}