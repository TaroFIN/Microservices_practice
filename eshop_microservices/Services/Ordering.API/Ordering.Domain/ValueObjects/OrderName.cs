namespace Ordering.Domain.ValueObjects;

public record OrderName
{
    private const int DefaultMaxLength = 50;
    public string Value { get; }
    private OrderName(string value) => Value = value;
    public static OrderName Of(string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);
        ArgumentOutOfRangeException.ThrowIfNotEqual(value.Length, DefaultMaxLength);
        return new OrderName(value);
    }
}

