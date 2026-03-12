using Ordering.Application.Orders.Commands.DeleteOrder;

namespace Ordering.API.Endpoints;

public record DeleteOrderResponse(bool IsSuccess);

public class DeleteOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/orders/{id:guid}", async (Guid id, ISender sender) =>
        {
            var command = new DeleteOrderCommand(id);
            var result = await sender.Send(command);
            return Results.Ok(new DeleteOrderResponse(result.IsSuccess));
        })
        .WithName("DeleteOrder")
        .Produces<DeleteOrderResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Order")
        .WithDescription("Deletes an order by ID");
    }
}
