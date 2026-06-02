namespace Application.Requests;

public sealed class UpdateProductRequest
{
    public string Name { get; init; } = null!;
    public decimal Price { get; init; }
    public string? Description { get; init; }
}
