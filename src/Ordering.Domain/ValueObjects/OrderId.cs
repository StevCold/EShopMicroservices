namespace Ordering.Domain.ValueObjects;

public sealed class OrderId : IEquatable<OrderId>
{
    public Guid Value { get; }
    
    private OrderId(Guid value) => Value = value;

    public static OrderId Of(Guid value)
    {
        if (value == Guid.Empty)
            throw new DomainException("OrderId cannot be empty");
        return new OrderId(value);
    }

    public bool Equals(OrderId? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => Equals(obj as OrderId);
    public override int GetHashCode() => Value.GetHashCode();
}
