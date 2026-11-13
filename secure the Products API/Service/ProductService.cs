
public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ProductDto>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();

        return products.Select(p => p.ToDto()).ToList();
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);

        return product?.ToDto();
    }

    public async Task<ProductDto> CreateAsync(
        ProductRequestDto request)
    {
        var product = await _repository.AddAsync(
            new Product
            {
                Name = request.Name,
                Price = request.Price
            }
        );

        return product.ToDto();
    }

    public async Task<ProductDto?> UpdateAsync(
        int id,
        ProductRequestDto request)
    {
        var product = await _repository.UpdateAsync(
            id,
            new Product
            {
                Name = request.Name,
                Price = request.Price
            }
        );

        return product?.ToDto();
    }

    public Task<bool> DeleteAsync(int id)
    {
        return _repository.DeleteAsync(id);
    }
}
