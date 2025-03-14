﻿using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Product;
using SnowFlake.Dtos.APIs.Shop.CreateShop;

namespace SnowFlake.Services;

public interface IShopService
{
    Task<ShopEntity> CreateAsync(ShopEntity shop);
    Task<ShopEntity> GetShopByPlayerRoomCode(string playerRoomCode);
    Task<ShopEntity> GetShopByHostRoomCode(string hostRoomCode);
    Task<bool> AddShopTokens(ShopEntity shop, int totalCost);
    Task<bool> MinusShopTokens(ShopEntity shop, int totalCost);
    //Task<bool> UpdateShopStock(ShopEntity shop, BuyProduct productEntity);

}
