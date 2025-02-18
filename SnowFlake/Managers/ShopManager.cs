using MongoDB.Bson;
using SnowFlake.Dtos;
using SnowFlake.Dtos.APIs.Image.DeleteImage;
using SnowFlake.Dtos.APIs.Image.GetImage;
using SnowFlake.Dtos.APIs.Shop.BuySnowFlake;
using SnowFlake.Dtos.APIs.Shop.CreateShop;
using SnowFlake.Dtos.APIs.Shop.GetShop;
using SnowFlake.Dtos.APIs.Shop.SellSnowFlake;
using SnowFlake.Dtos.APIs.Shop.UpdateShop;
using SnowFlake.Dtos.APIs.Team.UpdateTeam;
using SnowFlake.Dtos.APIs.Transaction.CreateTransaction;
using SnowFlake.Services;
using SnowFlake.Utilities;

namespace SnowFlake.Managers;

public class ShopManager : IShopManager
{
    private readonly IImageService _imageService;
    private readonly IShopService _shopService;
    private readonly ITeamService _teamService;
    private readonly IProductService _productService;
    private readonly ICartService _cartService;
    private readonly ITransactionService _transactionService;

    public ShopManager(IImageService imageService,
                       IShopService shopService,
                       ITeamService teamService,
                       IProductService productService,
                       ICartService cartService,
                       ITransactionService transactionService)
    {
        _imageService = imageService;
        _shopService = shopService;
        _teamService = teamService;
        _productService = productService;
        _cartService = cartService;
        _transactionService = transactionService;
    }

    public async Task<CreateShopResponse> CreateShop(CreateShopRequest createShopRequest)
    {
        var shop = new ShopEntity
        {
            Id = ObjectId.GenerateNewId().ToString(),
            HostRoomCode = createShopRequest.HostRoomCode,
            PlayerRoomCode = createShopRequest.PlayerRoomCode,
            Tokens = createShopRequest.Tokens,
            CreationDate = DateTime.Now,
            ModifiedDate = null
        };

        var shopResult = await _shopService.CreateAsync(shop);
        if (shopResult is null) return new CreateShopResponse
        {
            Success = false,
            Message = null
        };
        return new CreateShopResponse
        {
            Success = true,
            Message = shopResult
        };
    }
    public async Task<ExchangeProductsResponse> ManageIncomingShopOrder(ExchangeProductsRequest updateShopStockRequest)
    {
        var products = updateShopStockRequest.Products.Select(p => new ProductEntity
        {
            ProductName = p.ProductName,
            RemainingStock = p.Quantity,
        });

        var (shop, team) = await GetShopAndTeam(updateShopStockRequest.HostRoomCode,
                                                updateShopStockRequest.PlayerRoomCode,
                                                updateShopStockRequest.TeamNumber);

        if (shop is null) return new ExchangeProductsResponse
        {
            Success = false,
            Message = "Shop not found."
        };
        if (team is null) return new ExchangeProductsResponse
        {
            Success = false,
            Message = "Team not found."
        };

        foreach (var product in updateShopStockRequest.Products)
        {
            var shopProduct = (await _productService.GetProductsByOwnerId(shop.Id))
                                                    .FirstOrDefault(p => p.ProductName == product.ProductName);
            var teamProduct = (await _productService.GetProductsByOwnerId(team.Id))
                                                    .FirstOrDefault(p => p.ProductName == product.ProductName);

            var totalCost = product.Quantity * teamProduct.Price;
            if (team.Tokens < totalCost) return new ExchangeProductsResponse
            {
                Success = false,
                Message = "Insufficient tokens."
            };

            shopProduct.RemainingStock -= product.Quantity;
            teamProduct.RemainingStock += product.Quantity;

            var updatedShopProduct = await _productService.UpdateProduct(shopProduct);
            var updatedTeamProduct = await _productService.UpdateProduct(teamProduct);

            await _teamService.MinusTeamTokens(team, totalCost);
            await _shopService.AddShopTokens(shop, totalCost);

            _ = await _transactionService.CreateTransaction(new TransactionEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                RoundNumber = updateShopStockRequest.RoundNumber,
                TeamId = team.Id,
                ShopId = shop.Id,
                ProductId = updatedTeamProduct.Id,
                ProductName = updatedTeamProduct.ProductName,
                Quantity = product.Quantity,
                Total = totalCost,
                CreationDate = DateTime.Now,
                ModifiedDate = null
            });
        }

        foreach (var cartId in updateShopStockRequest.CartIds)
        {
            var cartItem = await _cartService.GetCartItemById(cartId);
            _ = await _cartService.DeleteCartItemAsync(cartItem);
        }
        
        return new ExchangeProductsResponse
        {
            Success = true,
            Message = "Order success successfully."
        };
    }

    private async Task<(ShopEntity shop, TeamEntity team)> GetShopAndTeam(string? hostRoomCode, string? playerRoomCode, int teamNumber)
    {
        var shop = new ShopEntity();
        var team = new TeamEntity();
        if (!string.IsNullOrWhiteSpace(hostRoomCode))
        {
            shop = await _shopService.GetShopByHostRoomCode(hostRoomCode);
            team = await _teamService.GetTeam(teamNumber, null, hostRoomCode);
        }
        if (!string.IsNullOrWhiteSpace(playerRoomCode))
        {
            shop = await _shopService.GetShopByPlayerRoomCode(playerRoomCode);
            team = await _teamService.GetTeam(teamNumber, playerRoomCode, null);
        }

        return (shop, team);
    }

    public async Task<GetShopResponse> GetShopByHostRoomCode(string hostRoomCode)
    {
        var shop = await _shopService.GetShopByHostRoomCode(hostRoomCode);
        if (shop is null) return new GetShopResponse
        {
            Success = false,
            Message = null
        };

        var products = await _productService.GetProductsByOwnerId(shop.Id);
        if (products is null) return new GetShopResponse
        {
            Success = false,
            Message = null
        };

        var shopResult = new ShopWithProducts
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

        return new GetShopResponse
        {
            Success = true,
            Message = shopResult
        };
    }

    public async Task<BuySnowflakeResponse> ManageSnowflakeOrder(BuySnowflakeRequest buySnowflakeRequest)
    {
        var image = await _imageService.GetImageByUrl(buySnowflakeRequest.ImageUrl, ImageBuyingStatus.Pending.Name);
        if (image is null) return new BuySnowflakeResponse
        {
            Success = false,
            Message = "Image not found."
        };

        if (buySnowflakeRequest.IsBuyingConfirm == false)
        {
            image.ImageBuyingStatus = ImageBuyingStatus.Rejected.Name;
            image = await _imageService.UpdateImage(image);
            return image is not null
                ? new BuySnowflakeResponse
                {
                    Success = true,
                    Message = "Image deletion process successful."
                }
                : new BuySnowflakeResponse
                {
                    Success = false,
                    Message = "Image deletion process unsuccessful."
                };
        }

        var (shop, team) = await GetShopAndTeam(buySnowflakeRequest.HostRoomCode,
                                                buySnowflakeRequest.PlayerRoomCode,
                                                buySnowflakeRequest.TeamNumber);

        if (team is null) return new BuySnowflakeResponse
        {
            Success = false,
            Message = "Team not found."
        };
        if (shop is null) return new BuySnowflakeResponse
        {
            Success = false,
            Message = "Shop not found."
        };

        image.Price = buySnowflakeRequest.Price;
        image.ImageBuyingStatus = ImageBuyingStatus.Sold.Name;
        image.ModifiedDate = DateTime.Now;

        var updatedImage = await _imageService.UpdateImage(image);
        if(updatedImage is null) return new BuySnowflakeResponse
        {
            Success = false,
            Message = "Image order process unsuccessful."
        };

        var updatedTeam = await _teamService.Update(new UpdateTeamRequest
        {
            HostRoomCode = team.HostRoomCode,
            Tokens = buySnowflakeRequest.Price,
            TeamNumber = team.TeamNumber
        });
        if (updatedTeam is null)
            return new BuySnowflakeResponse
            {
                Success = false,
                Message = "Tokens transfer to team incomplete."
            };

        _ = await _transactionService.CreateTransaction(new TransactionEntity
        {
            Id = ObjectId.GenerateNewId().ToString(),
            TeamId = team.Id,
            ShopId = shop.Id,
            ImageId = image.Id,
            ImageName = image.FileName,
            RoundNumber = buySnowflakeRequest.RoundNumber,
            Quantity = 1,
            Total = buySnowflakeRequest.Price,
            CreationDate = DateTime.Now,
            ModifiedDate = null
        });

        return new BuySnowflakeResponse
        {
            Success = true,
            Message = "Order processed successfully."
        };
    }

    
}
