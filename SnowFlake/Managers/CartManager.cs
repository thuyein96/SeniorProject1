using MongoDB.Bson;
using MongoDB.Driver;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Cart;
using SnowFlake.Dtos.APIs.Cart.GetTeamCartItems;
using SnowFlake.Dtos.APIs.Cart.RemoveCartItem;
using SnowFlake.Services;

namespace SnowFlake.Managers;

public class CartManager : ICartManager
{
    private readonly ICartService _cartService;

    public CartManager(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<GetTeamCartItemsResponse> AddToCart(AddCartItemRequest addCartItemRequest)
    {

        var cartItem = new CartEntity
        {
            Id = ObjectId.GenerateNewId().ToString(),
            HostRoomCode = addCartItemRequest.HostRoomCode,
            PlayerRoomCode = addCartItemRequest.PlayerRoomCode,
            ProductName = addCartItemRequest.ProductName,
            Price = addCartItemRequest.Price,
            Quantity = addCartItemRequest.Quantity,
            TeamNumber = addCartItemRequest.TeamNumber,
            CreationDate = DateTime.Now,
            ModifiedDate = null
        };

        var addedItem = await _cartService.CreateCartItemAsync(cartItem);

        var cartItems = await GetCartItemsByRoomCode(addCartItemRequest.HostRoomCode, addCartItemRequest.PlayerRoomCode, addCartItemRequest.TeamNumber);

        return cartItems;
    }


    public async Task<GetTeamCartItemsResponse> GetCartItemsByRoomCode(string? hostRoomCode, string? playerRoomCode, int teamNumber)
    {
        var cartItems = new List<CartEntity>();
        if (!string.IsNullOrWhiteSpace(hostRoomCode))
        {
            cartItems = await _cartService.GetTeamCartItemByHostRoomCodeAsync(hostRoomCode, teamNumber);
        }

        if (!string.IsNullOrWhiteSpace(playerRoomCode))
        {
            cartItems = await _cartService.GetTeamCartItemByPlayerRoomCodeAsync(playerRoomCode, teamNumber);
        }

        return cartItems is null || cartItems.Count <= 0
            ? new GetTeamCartItemsResponse
            {
                Success = false,
                Message = null
            }
            : new GetTeamCartItemsResponse
            {
                Success = true,
                Message = cartItems
            };
    }
    public async Task<RemoveCartItemResponse> RemoveFromCart(string cartId)
    {
        var cartItem = await _cartService.GetCartItemById(cartId);
        if(cartItem is null) return new RemoveCartItemResponse
        {
            Success = false,
            Message = "Cart item not found."
        };

        var removeResult = await _cartService.DeleteCartItemAsync(cartItem);

        return string.IsNullOrWhiteSpace(removeResult) ? new RemoveCartItemResponse
        {
            Success = false,
            Message = "Failed to remove cart item."
        } : new RemoveCartItemResponse
        {
            Success = true,
            Message = $"Cart item {cartItem.ProductName} is successfully removed."
        };
    }
}