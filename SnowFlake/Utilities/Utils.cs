using MongoDB.Bson;
namespace SnowFlake.Utilities;

public static class Utils
{
    public static bool IsValidObjectId(string id)
    {
        if(ObjectId.TryParse(id, out _)) return true;
        return false;
    }

    public static int ConvertToSeconds(string duration)
    {
        var time = duration.Split(':');
        return int.Parse(time[0]) * 60 + int.Parse(time[1]);
    }
}
