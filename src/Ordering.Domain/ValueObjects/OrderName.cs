namespace Ordering.Domain.ValueObjects;

public class OrderName
{
    private const int DefaultLength = 5;
    public string Value { get; }
    private OrderName(string value) => Value = value;
    public static OrderName Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        if (value.Length < DefaultLength)
        {
            throw new DomainException($"OrderName must be at least {DefaultLength} characters long");
        }
        return new OrderName(value);
    }
}

