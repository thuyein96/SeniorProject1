using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Image.GetImage;
using SnowFlake.Dtos.APIs.Shop.GetShop;
using SnowFlake.Dtos.APIs.Shop.SellSnowFlake;
using SnowFlake.Dtos.APIs.Shop.UpdateShop;
using SnowFlake.Services;

namespace SnowFlake.Managers;

public class ShopManager : IShopManager
{
    private readonly IImageService _imageService;
    private readonly IShopService _shopService;
    private readonly ITeamService _teamService;
    private readonly IProductService _productService;

    public ShopManager(IImageService imageService,
                       IShopService shopService,
                       ITeamService teamService,
                       IProductService productService)
    {
        _imageService = imageService;
        _shopService = shopService;
        _teamService = teamService;
        _productService = productService;
    }

    public async Task<string> ManageIncomingShopOrder(ExchangeProductsRequest updateShopStockRequest)
    {
        var products = updateShopStockRequest.Products.Select(p => new ProductEntity
        {
            ProductName = p.ProductName,
            RemainingStock = p.Quantity,
        });

        var shop = await _shopService.GetShopByHostRoomCode(updateShopStockRequest.HostRoomCode);
        if (shop is null) return string.Empty;

        var team = await _teamService.GetTeam(updateShopStockRequest.TeamNumber, null, updateShopStockRequest.HostRoomCode);
        if (team is null) return string.Empty;

        foreach (var product in updateShopStockRequest.Products)
        {
            var shopProduct = (await _productService.GetProductsByOwnerId(shop.Id))
                                                    .FirstOrDefault(p => p.ProductName == product.ProductName);
            var teamProduct = (await _productService.GetProductsByOwnerId(team.Id))
                                                    .FirstOrDefault(p => p.ProductName == product.ProductName);

            var totalCost = product.Quantity * teamProduct.Price;
            if (team.Tokens < totalCost) return string.Empty;

            shopProduct.RemainingStock -= product.Quantity;
            teamProduct.RemainingStock += product.Quantity;

            var updatedShopProduct = await _productService.UpdateProduct(shopProduct);
            var updatedTeamProduct = await _productService.UpdateProduct(teamProduct);

            await _teamService.MinusTeamTokens(team, totalCost);
            await _shopService.AddShopTokens(shop, totalCost);
        }
        
        return "Order processed successful.";
    }

    public async Task<ShopWithProducts> GetShopByHostRoomCode(string hostRoomCode)
    {
        var shop = await _shopService.GetShopByHostRoomCode(hostRoomCode);
        if (shop is null) return null;

        var products = await _productService.GetProductsByOwnerId(shop.Id);
        if (products is null) return null;

        return new ShopWithProducts
        {
            Id = shop.Id,
            HostRoomCode = shop.HostRoomCode,
            PlayerRoomCode = shop.PlayerRoomCode,
            Tokens = shop.Tokens,
            ShopStocks = products.Select(p => new Product
            {
                ProductName = p.ProductName,
                RemainingStock = p.RemainingStock,
                Price = p.Price
            }).ToList()
        };
    }

    //public async Task<string> ManageSnowflakeOrder(BuySnowflakeRequest buySnowflakeRequest)
    //{
    //    var team = await _teamService.GetTeam(buySnowflakeRequest.TeamNumber, buySnowflakeRequest.PlayerRoomCode, null);
    //    if (team is null) return string.Empty;

    //    var image = await _imageService.GetImage(new GetImageRequest { Id = buySnowflakeRequest.ImageId });
    //    if (image is null) return string.Empty;


    //}
}
