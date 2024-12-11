using Ardalis.SmartEnum;

namespace SnowFlake.Utilities;

public sealed class GameState : SmartEnum<GameState, string>
{
    public static readonly GameState TeamCreation = new("TeamCreation", nameof(TeamCreation));
    public static readonly GameState SnowFlakeCreation = new("SnowFlakeCreation", nameof(SnowFlakeCreation));
    public static readonly GameState ShopPeriod = new("ShopPeriod", nameof(ShopPeriod));
    public static readonly GameState Leaderboard = new("Leaderboard", nameof(Leaderboard));
    private GameState(string value, string name) : base(name, value)
    {
    }
}
