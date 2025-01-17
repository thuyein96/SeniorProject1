using SnowFlake.Services;

namespace SnowFlake.Managers
{
    public class LeaderboardManager
    {
        private readonly IShopService _shopService;
        private readonly IImageService _imageService;
        private readonly IPlaygroundService _playgroundService;

        public LeaderboardManager(IShopService shopService,
                                  IImageService imageService,
                                  IPlaygroundService playgroundService)
        {
            _shopService = shopService;
            _imageService = imageService;
            _playgroundService = playgroundService;
        }


    }
}
