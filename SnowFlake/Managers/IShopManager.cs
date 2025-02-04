﻿using SnowFlake.Dtos.APIs.Shop.GetShop;
using SnowFlake.Dtos.APIs.Shop.SellSnowFlake;
using SnowFlake.Dtos.APIs.Shop.UpdateShop;

namespace SnowFlake.Managers;

public interface IShopManager
{
    Task<string> ManageIncomingShopOrder(ExchangeProductsRequest updateShopStockRequest);
    Task<ShopWithProducts> GetShopByHostRoomCode(string hostRoomCode);
    Task<string> ManageSnowflakeOrder(BuySnowflakeRequest buySnowflakeRequest);
}
