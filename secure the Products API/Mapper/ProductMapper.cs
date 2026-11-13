
public static class ProductMapper
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto(
            product.Id,
            product.Name,
            product.Price
        );
    }

    public static Product ToDomain(this ProductRequestDto dto)
    {
        return new Product
        {
            Name = dto.Name,
            Price = dto.Price
        };
    }
}
