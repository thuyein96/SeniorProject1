using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace SnowFlake.Dtos;
[Collection("Players")]
public class PlayerItem : BaseEntity
{
    public string Name { get; set; }
    public string? TeamId { get; set; }
    public string? RoomCode { get; set; }
}