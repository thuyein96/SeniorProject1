using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Microsoft.AspNetCore.Mvc;

namespace SnowFlake.Dtos.APIs.Team.CreateTeam;

public class CreateTeamRequest
{
    public ObjectId Id { get; set; }
    public int TeamNumber { get; set; }
    public int MaxMembers { get; set; }
    public int Tokens { get; set; }
}