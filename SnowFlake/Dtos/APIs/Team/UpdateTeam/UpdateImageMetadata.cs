using MongoDB.Bson;

namespace SnowFlake.Dtos.APIs.Team.UpdateTeam
{
    public class UpdateImageMetadata
    {
        public ObjectId Id { get; set; }
        public string SnowFlakeImageUrls { get; set; }
        public string ImageBuyingStatus { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
