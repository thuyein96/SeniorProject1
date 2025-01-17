using SnowFlake.Services;

namespace SnowFlake.Managers;

public class ShopManager
{
    private readonly IImageService _imageService;

    public ShopManager(IImageService imageService,
                       IShopService shopService)
    {
        _imageService = imageService;
    }
}
