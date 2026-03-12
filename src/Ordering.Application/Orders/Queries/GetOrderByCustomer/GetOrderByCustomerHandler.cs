using Microsoft.EntityFrameworkCore;

namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer;

public class GetOrdersByCustomerHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersByCustomerQuery, GetOrdersByCustomerResult>
{
    public async Task<GetOrdersByCustomerResult> Handle(GetOrdersByCustomerQuery query, CancellationToken cancellationToken)
    {
        var customerId = CustomerId.Of(query.CustomerId);

        var orders = await dbContext.Orders
            .AsNoTracking()
            .Include(o => o.OrderItems)
            .Where(o => o.CustomerId == customerId)
            .OrderBy(o => o.Id)
            .ToListAsync(cancellationToken);

        var orderDtos = MapToDtos(orders);

        return new GetOrdersByCustomerResult(orderDtos);
    }

    private static List<OrderDto> MapToDtos(List<Order> orders)
    {
        var result = new List<OrderDto>();
        foreach (var order in orders)
        {
            var items = order.OrderItems.Select(oi => new OrderItemDto(
                oi.OrderId.Value, oi.ProductId.Value, oi.Quantity, oi.Price)).ToList();

            result.Add(new OrderDto(
                order.Id.Value,
                order.CustomerId.Value,
                order.OrderName.Value,
                new AddressDto(order.ShippingAddress.FirstName, order.ShippingAddress.LastName,
                    order.ShippingAddress.EmailAddress ?? "", order.ShippingAddress.AddressLine,
                    order.ShippingAddress.Country, order.ShippingAddress.State, order.ShippingAddress.ZipCode),
                new AddressDto(order.BillingAddress.FirstName, order.BillingAddress.LastName,
                    order.BillingAddress.EmailAddress ?? "", order.BillingAddress.AddressLine,
                    order.BillingAddress.Country, order.BillingAddress.State, order.BillingAddress.ZipCode),
                new PaymentDto(order.Payment.CardName ?? "", order.Payment.CardNumber,
                    order.Payment.Expiration, order.Payment.CVV, order.Payment.PaymentMethod),
                order.Status,
                items
            ));
        }
        return result;
    }
}
