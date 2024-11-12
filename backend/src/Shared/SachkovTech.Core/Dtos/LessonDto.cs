namespace SachkovTech.Core.Dtos;

public record LessonDto
{
    public Guid Id { get; init; }
    public Guid ModuleId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int Position { get; init; }
    public int Experience { get; init; }
    public string VideoUrl { get; init; } = string.Empty;
    public string PreviewUrl { get; init; } = string.Empty;
    public Guid[] Tags { get; init; } = [];
    public Guid[] Issues { get; init; } = [];
}