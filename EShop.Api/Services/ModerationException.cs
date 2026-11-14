namespace EShop.Api.Services;

public sealed class ModerationException : Exception
{
    public ModerationException(string message)
        : base(message)
    {
    }
}
