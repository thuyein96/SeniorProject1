﻿using SnowFlake.Dtos.APIs.Player;

namespace SnowFlake.Dtos.APIs;

public class CreatePlayerResponse
{
    public bool Success { get; set; }
    public PlayerItem Message { get; set; }
}