using SnowFlake.Dtos.APIs.Cart;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Cart.RemoveCartItem;

namespace SnowFlake.Managers;

public interface ICartManager
{
    Task<List<CartEntity>> AddToCart(AddCartItemRequest addCartItemRequest);
    Task<List<CartEntity>> GetCartItemsByRoomCode(string hostRoomCode, string playerRoomCode, int teamNumber);
    Task<string> RemoveCart(RemoveCartItemRequest removeCartItemRequest);
}