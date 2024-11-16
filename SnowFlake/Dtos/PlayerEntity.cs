using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace SnowFlake.Dtos;
[Collection("Players")]
public class PlayerEntity : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string StudentId { get; set; }
    public string? Major { get; set; }
    public string? Faculty { get; set; }
    public string? FirebaseId { get; set; }
    public ObjectId TeamId { get; set; }
    public string? ProfileImageUrl { get; set; }
}