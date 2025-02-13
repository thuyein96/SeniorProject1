using MongoDB.Bson;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Cart;
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

    public async Task<List<CartEntity>> AddToCart(AddCartItemRequest addCartItemRequest)
    {

        var cartItem = new CartEntity
        {
            Id = ObjectId.GenerateNewId().ToString(),
            HostRoomCode = addCartItemRequest.HostRoomCode,
            PlayerRoomCode = addCartItemRequest.PlayerRoomCode,
            ProductName = addCartItemRequest.ProductName,
            Price = addCartItemRequest.Price,
            Quantity = addCartItemRequest.Quantity,
            TeamNumber = addCartItemRequest.TeamNumber
        };

        var addedItem = await _cartService.CreateCartItemAsync(cartItem);

        var cartItems = await GetCartItemsByRoomCode(addCartItemRequest.HostRoomCode, addCartItemRequest.PlayerRoomCode, addCartItemRequest.TeamNumber);

        if (cartItems is null || cartItems.Count <= 0) return null;

        return cartItems;
    }


    public async Task<List<CartEntity>> GetCartItemsByRoomCode(string? hostRoomCode, string? playerRoomCode, int teamNumber)
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

        return cartItems;
    }

    public async Task<List<CartEntity>> RemoveCart(RemoveCartItemRequest removeCartItemRequest)
    {
        var cartItem = await _cartService.GetCartItemAsync(removeCartItemRequest.HostRoomCode,
                                                                     removeCartItemRequest.PlayerRoomCode,
                                                                     removeCartItemRequest.ProductName,
                                                                     removeCartItemRequest.TeamNumber);
        var removeResult = await _cartService.DeleteCartItemAsync(cartItem);

        var cartItems = await GetCartItemsByRoomCode(removeCartItemRequest.HostRoomCode, removeCartItemRequest.PlayerRoomCode, removeCartItemRequest.TeamNumber);
        return cartItems;
    }
}