namespace SnowFlake.Dtos.APIs.Cart;

public class AddCartItemResponse : BaseResponse
{
    public List<CartEntity> Message { get; set; }
}