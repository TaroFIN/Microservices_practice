namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, string Description, decimal Price, int Stock, List<string> Category)
    : ICommand<UpdateProductResult>;
public record UpdateProductResult(bool IsSuccess, string? ErrorMessage);

internal class UpdateProductHandler(IDocumentSession session, ILogger<UpdateProductHandler> logger)
    :ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating product {Id}", command.Id);
        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
        if (product is null)
        {
            return new UpdateProductResult(false, "Product not found");
        }
        product.Name = command.Name;
        product.Description = command.Description;
        product.Price = command.Price;
        product.Category = command.Category;

        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true, null);
    }
}

