using Ardalis.SmartEnum;

namespace SnowFlake.Utilities
{
    public sealed class RoundState : SmartEnum<RoundState>
    {
        public static readonly RoundState Pending = new RoundState(1, "Pending");
        public static readonly RoundState OnGoing = new RoundState(2, "OnGoing");
        public static readonly RoundState Finished = new RoundState(3, "Finished");

        private RoundState(int value, string name)
            : base(name, value)
        {
        }
    }
}
