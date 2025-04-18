﻿using BuildingBlocks.Exceptions;

namespace Catalog.API.Products.GetProductById;

//public record GetProductByIdRequest(Guid id) : IQuery<GetProductByIdResult>;
public record GetProductByIdResponse(Product product);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}", async (Guid id, ISender sender) =>
        {
            if (!Guid.TryParse(id.ToString(), out _))
            {
                throw new BadRequestException("Invalid id format.");
            }

            var result = await sender.Send(new GetProductByIdQuery(id));
            var response = result.Adapt<GetProductByIdResponse>();
            return Results.Ok(response);
        })
        .WithName("GetProductById")
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Product By Id")
        .WithDescription("Get Product By Id");
    }
}

