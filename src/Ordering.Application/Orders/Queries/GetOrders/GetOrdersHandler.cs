using BuildingBlocks.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Ordering.Application.Orders.Queries.GetOrders;

public class GetOrdersHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;

        // Get total count
        var totalCount = await dbContext.Orders.CountAsync(cancellationToken);

        // Get paginated orders
        var orders = await dbContext.Orders
            .AsNoTracking()
            .Include(o => o.OrderItems)
            .OrderBy(o => o.Id)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        // Map to DTOs
        var orderDtos = new List<OrderDto>();
        foreach (var order in orders)
        {
            var items = new List<OrderItemDto>();
            foreach (var item in order.OrderItems)
            {
                items.Add(new OrderItemDto(item.OrderId.Value, item.ProductId.Value, item.Quantity, item.Price));
            }

            orderDtos.Add(new OrderDto(
                order.Id.Value,
                order.CustomerId.Value,
                order.OrderName.Value,
                new AddressDto(
                    order.ShippingAddress.FirstName,
                    order.ShippingAddress.LastName,
                    order.ShippingAddress.EmailAddress ?? "",
                    order.ShippingAddress.AddressLine,
                    order.ShippingAddress.Country,
                    order.ShippingAddress.State,
                    order.ShippingAddress.ZipCode),
                new AddressDto(
                    order.BillingAddress.FirstName,
                    order.BillingAddress.LastName,
                    order.BillingAddress.EmailAddress ?? "",
                    order.BillingAddress.AddressLine,
                    order.BillingAddress.Country,
                    order.BillingAddress.State,
                    order.BillingAddress.ZipCode),
                new PaymentDto(
                    order.Payment.CardName ?? "",
                    order.Payment.CardNumber,
                    order.Payment.Expiration,
                    order.Payment.CVV,
                    order.Payment.PaymentMethod),
                order.Status,
                items
            ));
        }

        return new GetOrdersResult(new PaginatedResult<OrderDto>(pageIndex, pageSize, totalCount, orderDtos));
    }
}
