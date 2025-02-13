namespace SnowFlake.Dtos.APIs.Cart.RemoveCartItem;

public class RemoveCartItemResponse : BaseResponse
{
    public List<CartEntity> Message { get; set; }
}