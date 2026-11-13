
public interface IProductService
{
    Task<List<ProductDto>> GetAllAsync();

    Task<ProductDto?> GetByIdAsync(int id);

    Task<ProductDto> CreateAsync(ProductRequestDto request);

    Task<ProductDto?> UpdateAsync(int id, ProductRequestDto request);

    Task<bool> DeleteAsync(int id);
}
