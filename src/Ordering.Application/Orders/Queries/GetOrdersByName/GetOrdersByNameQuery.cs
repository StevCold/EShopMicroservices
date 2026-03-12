//namespace Ordering.Application.Orders.Queries.GetOrdersByName;

//public record GetOrdersByNameQuery(string Name)
//    : IQuery<GetOrdersByNameResult>;

//public record GetOrdersByNameResult(IEnumerable<OrderDto> Orders);


namespace Ordering.Application.Orders.Queries.GetOrdersByName;

public record GetOrdersByNameQuery(string Name)
    : IQuery<GetOrdersByNameResult>;

public record GetOrdersByNameResult(IEnumerable<OrderDto> Orders);
