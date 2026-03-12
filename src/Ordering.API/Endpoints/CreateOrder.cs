using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.API.Endpoints;

public record CreateOrderRequest(OrderDto Order);
public record CreateOrderResponse(Guid OrderId);

public class CreateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
        {
            var command = new CreateOrderCommand(request.Order);
            var result = await sender.Send(command);
            return Results.Created($"/orders/{result.Id}", new CreateOrderResponse(result.Id));
        })
        .WithName("CreateOrder")
        .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Order")
        .WithDescription("Creates a new order");
    }
}
