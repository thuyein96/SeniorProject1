using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Cart;
using SnowFlake.UnitOfWork;

namespace SnowFlake.Services;

public class CartService : ICartService
{
    private readonly IUnitOfWork _unitOfWork;

    public CartService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CartEntity> CreateCartItemAsync(CartEntity cartItem)
    {
        await _unitOfWork.CartRepository.Create(cartItem);
        await _unitOfWork.Commit();
        return cartItem;
    }

    public async Task<List<CartEntity>> GetCartItemsByPlayerRoomCodeAsync(string playerRoomCode)
    {
        var cartItems = (await _unitOfWork.CartRepository.GetBy(c => c.PlayerRoomCode == playerRoomCode)).ToList();

        return cartItems;
    }

    public async Task<List<CartEntity>> GetCartItemsByHostRoomCodeAsync(string hostRoomCode)
    {
        var cartItems = (await _unitOfWork.CartRepository.GetBy(c => c.HostRoomCode == hostRoomCode)).ToList();

        return cartItems;
    }

    public async Task<List<CartEntity>> GetTeamCartItemByHostRoomCodeAsync(string hostRoomCode, int teamNumber)
    {
        var cartItems =
            (await _unitOfWork.CartRepository.GetBy(c => c.HostRoomCode == hostRoomCode && c.TeamNumber == teamNumber))
            .ToList();

        return cartItems;
    }

    public async Task<List<CartEntity>> GetTeamCartItemByPlayerRoomCodeAsync(string playerRoomCode, int teamNumber)
    {
        var cartItems =
            (await _unitOfWork.CartRepository.GetBy(c => c.PlayerRoomCode == playerRoomCode && c.TeamNumber == teamNumber))
            .ToList();

        return cartItems;
    }

    public async Task<CartEntity> GetCartItemAsync(string hostRoomCode, string playerRoomCode, string productName, int teamNumber)
    {
        var cartItem = new CartEntity();
        if (!string.IsNullOrWhiteSpace(hostRoomCode))
        {
            cartItem = (await _unitOfWork.CartRepository.GetBy(c => c.HostRoomCode == hostRoomCode && c.ProductName == productName && c.TeamNumber == teamNumber)).FirstOrDefault();
        }

        if (!string.IsNullOrWhiteSpace(playerRoomCode))
        {
            cartItem = (await _unitOfWork.CartRepository.GetBy(c =>
                    c.PlayerRoomCode == playerRoomCode && c.ProductName == productName && c.TeamNumber == teamNumber))
                .FirstOrDefault();
        }
        return cartItem;
    }

    public async Task<CartEntity> GetCartItemById(string cartId)
    {
        var cartItem = (await _unitOfWork.CartRepository.GetBy(c => c.Id == cartId)).FirstOrDefault();
        return cartItem;
    }
    public async Task<string> DeleteCartItemAsync(CartEntity cartItem)
    {
        await _unitOfWork.CartRepository.Delete(cartItem);
        await _unitOfWork.Commit();

        return $"Cart Item {cartItem.Id} is successfully deleted.";
    }
}