namespace Core.Entities;

public sealed class Product
{
    private Product() { }

    public Product(string name, decimal price, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Product name is required.", nameof(name));
        }

        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Price must be greater than or equal to zero.");
        }

        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public decimal Price { get; private set; }
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Product name is required.", nameof(name));
        }

        Name = name;
    }

    public void ChangePrice(decimal price)
    {
        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Price must be greater than or equal to zero.");
        }

        Price = price;
    }

    public void ChangeDescription(string? description)
    {
        Description = description;
    }
}
