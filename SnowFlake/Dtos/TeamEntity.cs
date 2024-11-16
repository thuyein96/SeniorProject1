using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

namespace SnowFlake.Dtos;

[Collection("Teams")]
public class TeamEntity : BaseEntity
{
    public string TeamNumber { get; set; }
    public int MaxMembers { get; set; }
    public int Tokens { get; set; }
}