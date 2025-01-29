using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Product.UpdateShop;
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

    public async Task<string> ManageIncomingShopOrder(UpdateStockRequest updateShopStockRequest)
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
            var shopProduct = (await _productService.GetProductByOwnerId(shop.Id))
                                                    .FirstOrDefault(p => p.ProductName == product.ProductName);
            var teamProduct = (await _productService.GetProductByOwnerId(team.Id))
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
}
