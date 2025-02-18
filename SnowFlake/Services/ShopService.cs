using MongoDB.Bson;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Product;
using SnowFlake.Dtos.APIs.Shop.CreateShop;
using SnowFlake.UnitOfWork;

namespace SnowFlake.Services;

public class ShopService : IShopService
{
    private readonly IUnitOfWork _unitOfWork;

    public ShopService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ShopEntity> CreateAsync(ShopEntity shop)
    {
        try
        {
            await _unitOfWork.ShopRepository.Create(shop);
            await _unitOfWork.Commit();

            return shop;
        }
        catch (Exception)
        {

            return null;
        }
    }

    public async Task<ShopEntity> GetShopByPlayerRoomCode(string playerRoomCode)
    {
        try
        {
            var shop = (await _unitOfWork.ShopRepository.GetBy(s => s.PlayerRoomCode == playerRoomCode)).FirstOrDefault();
            return shop;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<ShopEntity> GetShopByHostRoomCode(string hostRoomCode)
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

    public async Task<bool> AddShopTokens(ShopEntity shop, int totalCost)
    {
        try
        {
            if (shop is null || totalCost <= 0) return false;

            shop.Tokens += totalCost;
            shop.ModifiedDate = DateTime.Now;

            await _unitOfWork.ShopRepository.Update(shop);
            await _unitOfWork.Commit();

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> MinusShopTokens(ShopEntity shop, int totalCost)
    {
        try
        {
            if (shop is null || totalCost <= 0) return false;

            shop.Tokens -= totalCost;
            shop.ModifiedDate = DateTime.Now;

            await _unitOfWork.ShopRepository.Update(shop);
            await _unitOfWork.Commit();

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    //public async Task<bool> UpdateShopStock(ShopEntity shop, BuyProduct soldProductEntity)
    //{
    //    try
    //    {
    //        var shopStocks = shop.ShopStocks;
    //        var shopStock = shopStocks.FirstOrDefault(s => s.ProductName == soldProductEntity.ProductName);
    //        if (shopStock is null || shopStock.RemainingStock < soldProductEntity.Quantity) return false;

    //        shopStock.RemainingStock -= soldProductEntity.Quantity;
    //        shop.Tokens += soldProductEntity.Quantity * shopStock.Price;

    //        shop.ShopStocks = shopStocks;
    //        shop.ModifiedDate = DateTime.Now;

    //        await _unitOfWork.ShopRepository.Update(shop);
    //        await _unitOfWork.Commit();

    //        return true;
    //    }
    //    catch (Exception)
    //    {
    //        return false;
    //    }
    //}
}
