namespace EShop.Api.DTO;

public record CommentResponse(
    int Id,
    int ItemId,
    string Text,
    DateTime CreatedAt,
    bool IsOwner
);