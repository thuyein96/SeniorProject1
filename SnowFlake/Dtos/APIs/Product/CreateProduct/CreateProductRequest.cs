namespace SnowFlake.Dtos.APIs.Product.CreateProduct;

public class CreateProductRequest
{
    public string ProductName { get; set; }
    public int Price { get; set; }
    public int RemainingStocks { get; set; }
}