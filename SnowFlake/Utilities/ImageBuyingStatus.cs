﻿using Ardalis.SmartEnum;

namespace SnowFlake.Utilities;

public class ImageBuyingStatus : SmartEnum<ImageBuyingStatus>
{
    public static readonly ImageBuyingStatus Pending = new ImageBuyingStatus(1, "Pending");
    public static readonly ImageBuyingStatus Bought = new ImageBuyingStatus(2, "Bought");
    public static readonly ImageBuyingStatus Rejected = new ImageBuyingStatus(3, "Rejected");
    private ImageBuyingStatus(int value, string name)
        : base(name, value)
    {
    }
}