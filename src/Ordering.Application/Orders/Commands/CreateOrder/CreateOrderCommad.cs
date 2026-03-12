using BuildingBlocks.CQRS;
using FluentValidation;
using Ordering.Application.Dtos;
using System.Windows.Input;

namespace Ordering.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(OrderDto Order)
    : ICommand<CreateOrderResult>;

public record CreateOrderResult(Guid Id);

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Order.OrderName).NotNull().WithMessage("Order name is required.");
        RuleFor(x => x.Order.CustomerId).NotEmpty().WithMessage("Customer ID is required.");
        RuleFor(x => x.Order.OrderItems).NotEmpty().WithMessage("Order must have at least one item.");
    }
}