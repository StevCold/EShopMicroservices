using Marten.Schema;

namespace Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();
        if (await session.Query<Product>().AnyAsync())
            return;

        // Marten UPSET will cater for existing records
        session.Store<Product>(GetPreconfiguratedProducts());
        await session.SaveChangesAsync();
    }

    private IEnumerable<Product> GetPreconfiguratedProducts() => new List<Product>()
    {
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "IPhone X",
            Category = new List<string>() { "Smart Phone" },
            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
            ImageFile = "product-1.png",
            Price = 950.00M
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Samsung 10",
            Category = new List<string>() { "Smart Phone" },
            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
            ImageFile = "product-2.png",
            Price = 840.00M
        },
        new Product()
           {
            Id = Guid.NewGuid(),
            Name = "Huawei Plus",
            Category = new List<string>() { "Smart Phone" },
            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
            ImageFile = "product-3.png",
            Price = 650.00M
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Xiaomi Mi 9",
            Category = new List<string>() { "Smart Phone" },
            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
            ImageFile = "product-4.png",
            Price = 470.00M
        }
    };
}

