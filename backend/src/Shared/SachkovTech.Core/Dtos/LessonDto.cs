﻿namespace SachkovTech.Core.Dtos;

public record LessonDto
{
    public Guid Id { get; init; }
    public Guid ModuleId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int Experience { get; init; }
    public Guid VideoId { get; init; }
    public Guid PreviewFileId { get; init; }
    public Guid[] Tags { get; init; }
    public Guid[] Issues { get; init; }
}
