using MongoDB.Bson;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Shop.CreateShop;
using SnowFlake.Dtos.APIs.Shop.UpdateShop;
using SnowFlake.UnitOfWork;

namespace SnowFlake.Services;

public class ShopService : IShopService
{
    private readonly IUnitOfWork _unitOfWork;

    public ShopService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ShopEntity> CreateAsync(CreateShopRequest createShopRequest)
    {
        try
        {
            var shop = new ShopEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                HostRoomCode = createShopRequest.HostRoomCode,
                PlayerRoomCode = createShopRequest.PlayerRoomCode,
                Tokens = createShopRequest.Tokens,
                ShopStocks = createShopRequest.ShopStocks
            };
            await _unitOfWork.ShopRepository.Create(shop);
            return shop;
        }
        catch (Exception)
        {

            return null;
        }
    }

    public async Task<ShopEntity> GetShopByHostRoomCodeAsync(string hostRoomCode)
    {
        try
        {
            var shop = (await _unitOfWork.ShopRepository.GetBy(s => s.HostRoomCode == hostRoomCode)).FirstOrDefault();
            return shop;
        }
        catch (Exception)
        {

            return null;
        }
    }

    public async Task<string> UpdateStockAsync(UpdateStockRequest updateStockRequest)
    {
        try
        {
            var existingShop = (await _unitOfWork.ShopRepository.GetBy(s => s.HostRoomCode == updateStockRequest.HostRoomCode)).FirstOrDefault();

            if (existingShop is null) return string.Empty;

            var shopStock = existingShop.ShopStocks.FirstOrDefault(s => s.ProductName == updateStockRequest.SoldProduct);

            if (shopStock is null || shopStock.RemainingStock < updateStockRequest.Quantity) return string.Empty;

            shopStock.RemainingStock -= updateStockRequest.Quantity;
            existingShop.Tokens += updateStockRequest.Quantity * shopStock.Price;

            var existingTeam = (await _unitOfWork.TeamRepository.GetBy(t => (t.HostRoomCode == updateStockRequest.HostRoomCode) && (t.TeamNumber == updateStockRequest.TeamNumber))).FirstOrDefault();

            if (existingTeam is null) return string.Empty;

            var teamStock = existingTeam.TeamStocks.Where(t => t.ProductName == updateStockRequest.SoldProduct).FirstOrDefault();

            if (teamStock is null) return string.Empty;

            teamStock.RemainingStock += updateStockRequest.Quantity;
            existingTeam.Tokens -= updateStockRequest.Quantity * shopStock.Price;

            await _unitOfWork.ShopRepository.Update(existingShop);
            await _unitOfWork.TeamRepository.Update(existingTeam);

            await _unitOfWork.Commit();

            return "Stock updated successfully";
        }
        catch (Exception)
        {

            return string.Empty;
        }
    }
}
