using SnowFlake.Dtos.APIs.Product.CreateProduct;

namespace SnowFlake.Dtos.APIs.Product.CreateProducts;

public class CreateProductsRequest
{
    public List<CreateProductRequest> Products { get; set; }
}