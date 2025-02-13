namespace SnowFlake.Dtos.APIs.Cart.GetTeamCartItems;

public class GetTeamCartItemsResponse : BaseResponse
{
    public List<CartEntity> Message { get; set; }
}