
public class ProductRepository : IProductRepository
{
    private static readonly List<Product> _products = new();
    private static int _nextId = 1;

    public Task<List<Product>> GetAllAsync()
    {
        return Task.FromResult(_products);
    }

    public Task<Product?> GetByIdAsync(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product);
    }

    public Task<Product> AddAsync(Product product)
    {
        product.Id = _nextId++;
        _products.Add(product);

        return Task.FromResult(product);
    }

    public Task<Product?> UpdateAsync(int id, Product product)
    {
        var existing = _products.FirstOrDefault(p => p.Id == id);

        if (existing == null)
            return Task.FromResult<Product?>(null);

        existing.Name = product.Name;
        existing.Price = product.Price;

        return Task.FromResult<Product?>(existing);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);

        if (product == null)
            return Task.FromResult(false);

        _products.Remove(product);

        return Task.FromResult(true);
    }
}
