using Ardalis.SmartEnum;

namespace SnowFlake.Utilities
{
    public class GameProgress : SmartEnum<GameProgress>
    {
        public static readonly GameProgress Pending = new GameProgress(1, "Pending");
        public static readonly GameProgress OnGoing = new GameProgress(2, "OnGoing");
        public static readonly GameProgress Finished = new GameProgress(3, "Finished");

        private GameProgress(int value, string name)
            : base(name, value)
        {
        }
    }
}
