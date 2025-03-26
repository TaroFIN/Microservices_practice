namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

internal class DeleteProductCommandHandler(IDocumentSession session, ILogger<DeleteProductCommandHandler> logger)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting product {Id}", command.id);
        //var product = await session.LoadAsync<Product>(command.id, cancellationToken);
        //if (product is null)
        //{
        //    return new DeleteProductResult(false);
        //}
        session.Delete<Product>(command.id);
        await session.SaveChangesAsync(cancellationToken);
        return new DeleteProductResult(true);
    }
}

