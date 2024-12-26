namespace SnowFlake.Hubs.ConnectIDHandler
{
    public static class UserHandler
    {
        // just remember when you will restart the app, object will get reset
        public static HashSet<string> ConnectedIds = new HashSet<string>();
    }
}
