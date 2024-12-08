using Ardalis.SmartEnum;

namespace SnowFlake.Utilities
{
    public class TimerStatus : SmartEnum<TimerStatus>
    {
        public static readonly TimerStatus Running = new TimerStatus(nameof(Running), 1);
        public static readonly TimerStatus Paused = new TimerStatus(nameof(Paused), 2);
        public static readonly TimerStatus Stopped = new TimerStatus(nameof(Stopped), 3);
        private TimerStatus(string name, int value) : base(name, value)
        {
        }
    }
}
