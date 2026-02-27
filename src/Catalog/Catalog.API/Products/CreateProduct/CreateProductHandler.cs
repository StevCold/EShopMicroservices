namespace Catalog.API.Products.CreateProduct;
using System.Threading;
using System.Threading.Tasks;


public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    :ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);

internal class CreateProductComamandHandler(IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // Business logic to create a product
        //save to database
        //return CreateProductResult result
        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price,
        };
        // TODO
        //save to database
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);


        //return result
        return new CreateProductResult(product.Id);
    }
}

