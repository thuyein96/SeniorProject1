namespace SnowFlake.Dtos.APIs.Product.GetProducts;

public class GetProductsResponse : BaseResponse
{
    public List<ProductEntity> Message { get; set; }
}