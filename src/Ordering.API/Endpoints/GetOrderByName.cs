using Ordering.Application.Orders.Queries.GetOrdersByName;

namespace Ordering.API.Endpoints;

public record GetOrdersByNameResponse(IEnumerable<OrderDto> Orders);

public class GetOrderByName : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/name/{orderName}", async (string orderName, ISender sender) =>
        {
            var query = new GetOrdersByNameQuery(orderName);
            var result = await sender.Send(query);
            return Results.Ok(result);
        })
        .WithName("GetOrderByName")
        .Produces<GetOrdersByNameResult>(StatusCodes.Status200OK)
        .WithSummary("Get Orders By Name")
        .WithDescription("Gets orders matching the specified name");
    }
}
