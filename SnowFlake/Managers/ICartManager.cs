using SnowFlake.Dtos.APIs.Cart;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Cart.GetTeamCartItems;
using SnowFlake.Dtos.APIs.Cart.RemoveCartItem;

namespace SnowFlake.Managers;

public interface ICartManager
{
    Task<GetTeamCartItemsResponse> AddToCart(AddCartItemRequest addCartItemRequest);
    Task<GetTeamCartItemsResponse> GetCartItemsByRoomCode(string hostRoomCode, string playerRoomCode, int teamNumber);
    Task<RemoveCartItemResponse> RemoveFromCart(string cartId);
}