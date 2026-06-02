using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data;

public static class AppDbContextSeed
{
    public static async Task SeedAsync(AppDbContext context, ILogger logger, CancellationToken cancellationToken = default)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        logger.LogInformation("Applying database migrations...");
        await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);

        if (await context.Products.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            logger.LogInformation("Database already contains product seed data.");
            return;
        }

        logger.LogInformation("Seeding initial product data...");

        var products = new List<Product>
        {
            new Product("Widget", 19.99m, "A production-ready widget."),
            new Product("Gadget", 29.99m, "A useful gadget for everyday tasks."),
            new Product("Service Plan", 99.00m, "Annual support and maintenance.")
        };

        await context.Products.AddRangeAsync(products, cancellationToken).ConfigureAwait(false);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Database seeding completed.");
    }
}
