namespace EShop.Api.Services;

public enum ModerationReason
{
    DuplicateItemReport,
    DuplicateCommentReport,
    SelfCommentReport
}

public sealed class ModerationException : Exception
{
    public ModerationReason Reason { get; }

    public ModerationException(
        ModerationReason reason,
        string message,
        Exception? innerException = null)
        : base(message, innerException)
    {
        Reason = reason;
    }

    public static ModerationException DuplicateItemReport(Exception? innerException = null) =>
        new(
            ModerationReason.DuplicateItemReport,
            "Apie šį skelbimą jau pranešėte.",
            innerException);

    public static ModerationException DuplicateCommentReport(Exception? innerException = null) =>
        new(
            ModerationReason.DuplicateCommentReport,
            "Apie šį komentarą jau pranešėte.",
            innerException);

    public static ModerationException SelfCommentReport() =>
        new(
            ModerationReason.SelfCommentReport,
            "Negalite pranešti apie savo komentarą.");
}
