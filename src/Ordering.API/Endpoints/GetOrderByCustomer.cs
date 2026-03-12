using Ordering.Application.Orders.Queries.GetOrdersByCustomer;

namespace Ordering.API.Endpoints;

public record GetOrdersByCustomerResponse(IEnumerable<OrderDto> Orders);

public class GetOrdersByCustomer : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/customer/{customerId:guid}", async (Guid customerId, ISender sender) =>
        {
            var query = new GetOrdersByCustomerQuery(customerId);
            var result = await sender.Send(query);
            return Results.Ok(result);
        })
        .WithName("GetOrdersByCustomer")
        .Produces<GetOrdersByCustomerResult>(StatusCodes.Status200OK)
        .WithSummary("Get Orders By Customer")
        .WithDescription("Gets orders for a specific customer");
    }
}
