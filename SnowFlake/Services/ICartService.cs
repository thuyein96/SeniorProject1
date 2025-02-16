using SnowFlake.Dtos;

namespace SnowFlake.Services;

public interface ICartService
{
    Task<CartEntity> CreateCartItemAsync(CartEntity cartItem);
    Task<List<CartEntity>> GetCartItemsByHostRoomCodeAsync(string hostRoomCode);
    Task<List<CartEntity>> GetCartItemsByPlayerRoomCodeAsync(string playerRoomCode);
    Task<List<CartEntity>> GetTeamCartItemByHostRoomCodeAsync(string hostRoomCode, int teamNumber);
    Task<List<CartEntity>> GetTeamCartItemByPlayerRoomCodeAsync(string playerRoomCode, int teamNumber);
    Task<CartEntity> GetCartItemAsync(string hostRoomCode, string playerRoomCode, string productName, int teamNumber);
    Task<CartEntity> GetCartItemById(string cartId);
    Task<string> DeleteCartItemAsync(CartEntity cartItem);
}