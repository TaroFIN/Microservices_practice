namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

public class DeleteProductRequestValidator : AbstractValidator<DeleteProductRequest>
{
    public DeleteProductRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required.");
    }
}

internal class DeleteProductCommandHandler(IDocumentSession session)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(command.id, cancellationToken);
        if (product is null)
        {
            return new DeleteProductResult(false);
        }
        session.Delete<Product>(command.id);
        await session.SaveChangesAsync(cancellationToken);
        return new DeleteProductResult(true);
    }
}

