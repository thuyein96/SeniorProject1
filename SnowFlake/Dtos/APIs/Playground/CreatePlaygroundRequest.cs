﻿using MongoDB.Bson;

namespace SnowFlake.Dtos.APIs.Playground;

public class CreatePlaygroundRequest
{
    public string HostRoomCode { get; set; }
    public string PlayerRoomCode { get; set; }
    public List<RoundEntity> Rounds { get; set; }
    public int NumberOfTeam { get; set; }
    public int TeamToken { get; set; }
    //public int ShopToken { get; set; }
    //public List<ProductEntity> Shop { get; set; }

}