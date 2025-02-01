using SnowFlake.Dtos;

namespace SnowFlake.Services;

public interface IProductService
{
    Task<List<ProductEntity>> GetAllProducts();
    Task<ProductEntity> GetProductById(string productId);
    Task<ProductEntity> GetProductByName(string productName);
    Task<List<ProductEntity>> GetProductsByOwnerId(string ownerId);
    Task<ProductEntity> CreateProduct(ProductEntity product);
    Task<List<ProductEntity>> CreateAllThreeProducts(List<ProductEntity> products);
    Task<ProductEntity> UpdateProduct(ProductEntity product);
    Task<string> DeleteProduct(string productId);
}