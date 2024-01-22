namespace Shared.Helper;

public static class AppState
{
    public static Guid UserId { get; set; }
    public static bool IsAuthenticated { get; set; } = false;
}
