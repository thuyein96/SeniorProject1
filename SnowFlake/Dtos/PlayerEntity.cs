using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace SnowFlake.Dtos;
[Collection("Players")]
public class PlayerEntity : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string StudentId { get; set; } // delete
    public string? Major { get; set; } // delete
    public string? Faculty { get; set; } // delete
    public string? FirebaseId { get; set; } // delete
    public ObjectId TeamId { get; set; }
    public string? ProfileImageUrl { get; set; } // delete
}