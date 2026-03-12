using BuildingBlocks.Pagination;
using Ordering.Application.Orders.Queries.GetOrders;

namespace Ordering.API.Endpoints;

public record GetOrdersResponse(PaginatedResult<OrderDto> Orders);

public class GetOrders : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders", async ([AsParameters] PaginationRequest request, ISender sender) =>
        {
            var query = new GetOrdersQuery(request);
            var result = await sender.Send(query);
            return Results.Ok(result);
        })
        .WithName("GetOrders")
        .Produces<GetOrdersResult>(StatusCodes.Status200OK)
        .WithSummary("Get Orders")
        .WithDescription("Get paginated list of orders");
    }
}
