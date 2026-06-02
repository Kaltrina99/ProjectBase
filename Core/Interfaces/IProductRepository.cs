using Core.Entities;

namespace Core.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<IReadOnlyList<Product>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
