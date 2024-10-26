﻿namespace EmailNotificationService.API;

public record Response
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }
}
