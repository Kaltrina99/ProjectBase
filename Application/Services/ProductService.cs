using Application.DTOs;
using Application.Interfaces;
using Application.Requests;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public sealed class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<ProductDto> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var product = new Product(request.Name, request.Price, request.Description);

        await _productRepository.AddAsync(product, cancellationToken).ConfigureAwait(false);
        _logger.LogInformation("Created product {ProductId} with name {ProductName}", product.Id, product.Name);

        return Map(product);
    }

    public async Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.ListAsync(cancellationToken).ConfigureAwait(false);
        return products.Select(Map).ToList();
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
        return product is null ? null : Map(product);
    }

    public async Task<ProductDto> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var product = await _productRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
        if (product is null)
        {
            throw new InvalidOperationException($"Product with id '{id}' was not found.");
        }

        product.Rename(request.Name);
        product.ChangePrice(request.Price);
        product.ChangeDescription(request.Description);

        await _productRepository.UpdateAsync(product, cancellationToken).ConfigureAwait(false);
        _logger.LogInformation("Updated product {ProductId}", product.Id);

        return Map(product);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
        if (product is null)
        {
            throw new InvalidOperationException($"Product with id '{id}' was not found.");
        }

        await _productRepository.DeleteAsync(product, cancellationToken).ConfigureAwait(false);
        _logger.LogInformation("Deleted product {ProductId}", product.Id);
    }

    private static ProductDto Map(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Price = product.Price,
        Description = product.Description,
        CreatedAt = product.CreatedAt
    };
}
