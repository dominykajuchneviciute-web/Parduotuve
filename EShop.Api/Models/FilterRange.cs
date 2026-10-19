namespace EShop.Api.Models;

public readonly struct FilterRange
{
    public int? Minimum { get; }
    public int? Maximum { get; }

    public FilterRange(int? minimum = null, int? maximum = null)
    {
        if (minimum.HasValue && maximum.HasValue && minimum > maximum)
        {
            throw new ArgumentException("Minimum cannot be greater than maximum.");
        }

        Minimum = minimum;
        Maximum = maximum;
    }

    public bool IsSpecified => Minimum.HasValue || Maximum.HasValue;

    public bool Contains(int value)
    {
        return (!Minimum.HasValue || value >= Minimum.Value)
            && (!Maximum.HasValue || value <= Maximum.Value);
    }
}
