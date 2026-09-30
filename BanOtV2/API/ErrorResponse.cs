namespace API;

/// <summary>
/// Phản hồi lỗi thống nhất theo RFC 7807 và có thêm thời điểm phát sinh lỗi.
/// </summary>
public sealed class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public string? Instance { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? TraceId { get; set; }
}
