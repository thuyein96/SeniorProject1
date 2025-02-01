//using SnowFlake.Azure.BlobsStorageService;
//using SnowFlake.Dtos.APIs.Image.GetImage;
//using SnowFlake.Dtos.APIs.Image.UpdateImage;
//using SnowFlake.Dtos.APIs.Team.GetTeamsByRoomCode;
//using SnowFlake.Services;

//namespace SnowFlake.Managers;

//public class ImageManager : IImageManager
//{
//    private readonly IBlobStorageService _blobStorageService;
//    private readonly IImageService _imageService;
//    private readonly IShopService _shopService;
//    private readonly ITeamService _teamService;

//    public ImageManager(IBlobStorageService blobStorageService,
//                        IImageService imageService,
//                        IShopService shopService,
//                        ITeamService teamService)
//    {
//        _blobStorageService = blobStorageService;
//        _imageService = imageService;
//        _shopService = shopService;
//        _teamService = teamService;
//    }

//    public async Task SellImage()
//    {
//        // Get Image
//        var image = await _imageService.GetImage(new GetImageRequest
//        {
//            Id = ;
//        });
//        // Get ShopId
//        var shop = await _shopService.GetShopByHostRoomCode();
//        // Get TeamId
//        var team = await _teamService.GetTeamsByRoomCode(new GetTeamsByRoomCodeRequest
//        {
//            HostRoomCode = ;
//        });
//        // Update OwnerId from TeamId to ShopId in image
//        image.OwnerId = shop.Id;
//        image.ModifiedDate = DateTime.Now;
//        // Update Image
//        var isUpdated = await _imageService.UpdateImageOwner(image);
//        // Add Tokens to team
//        if(isUpdated)
//        {
//            await _shopService.AddShopTokens(shop, image.Price);
//        }
//        // Minus Tokens from shop
//        // Update Shop
//        // Update Team
//    }
//}