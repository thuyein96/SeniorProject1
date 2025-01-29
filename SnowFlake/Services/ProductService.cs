using SnowFlake.Dtos;
using SnowFlake.UnitOfWork;

namespace SnowFlake.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<ProductEntity>> GetAllProducts()
    {
        return (await _unitOfWork.ProductRepository.GetAll()).ToList();
    }

    public async Task<ProductEntity> GetProductById(string productId)
    {
        return (await _unitOfWork.ProductRepository.GetBy(p => p.Id == productId)).FirstOrDefault();
    }

    public async Task<ProductEntity> GetProductByName(string productName)
    {
        return (await _unitOfWork.ProductRepository.GetBy(p => p.ProductName == productName)).FirstOrDefault();
    }

    public async Task<List<ProductEntity>> GetProductByOwnerId(string ownerId)
    {
        return (await _unitOfWork.ProductRepository.GetBy(p => p.OwnerId == ownerId)).ToList();
    }

    public async Task<ProductEntity> CreateProduct(ProductEntity product)
    {
        await _unitOfWork.ProductRepository.Create(product);
        await _unitOfWork.Commit();
        return product;
    }

    public async Task<List<ProductEntity>> CreateAllThreeProducts(List<ProductEntity> products)
    {
        foreach (var product in products)
        {
            await _unitOfWork.ProductRepository.Create(product);
            await _unitOfWork.Commit();
        }
        
        return products;
    }

    public async Task<ProductEntity> UpdateProduct(ProductEntity product)
    {
        await _unitOfWork.ProductRepository.Update(product);
        await _unitOfWork.Commit();
        return product;
    }

    public async Task<string> DeleteProduct(string productId)
    {
        var product = await GetProductById(productId);
        if (product is null) return string.Empty;
        await _unitOfWork.ProductRepository.Delete(product);
        await _unitOfWork.Commit();
        return "Product deleted successfully.";
    }
}