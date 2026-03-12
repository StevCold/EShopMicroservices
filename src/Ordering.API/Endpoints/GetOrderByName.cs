
namespace Ordering.API.Endpoints;
public record GetOrderByNameRequest(string Name);
public record GetOrderByNameResult(bool IsSuccess);
public class GetOrderByName : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/name/{orderName}", async (string orderName, ISender sender) =>
        {
            var result = await sender.Send(orderName);
            var response = result.Adapt<GetOrderByNameResult>();
            return Results.Ok(response);
        })
        .WithName("GetOrderByName")
        .Produces<GetOrderByNameResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Order By Name")
        .WithDescription("Retrieves an order by its name.");
    }
}
