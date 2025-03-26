namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
public record GetProductByCategoryResult(IEnumerable<Product> Products);

public class GetProductByCategoryQueryHandler(IDocumentSession session, ILogger<GetProductByCategoryQueryHandler> logger)
    : IRequestHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductByCategoryQueryHandler.Handle called with {@Query}", request);

        var products = await session.Query<Product>()
            .Where(p => p.Category.Contains(request.Category))
            .ToListAsync(cancellationToken);
        return new GetProductByCategoryResult(products);
    }
}

