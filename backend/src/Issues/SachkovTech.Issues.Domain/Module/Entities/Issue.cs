using CSharpFunctionalExtensions;
using SachkovTech.Issues.Domain.Module.ValueObjects;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects;
using SachkovTech.SharedKernel.ValueObjects.Ids;
using FileInfo = SachkovTech.SharedKernel.ValueObjects.Ids.FileInfo;

namespace SachkovTech.Issues.Domain.Module.Entities;

public class Issue : SoftDeletableEntity<IssueId>
{
    private List<FileInfo> _filesInfo = [];

    //ef core navigation
    public Module Module { get; private set; } = null!;

    //ef core
    private Issue(IssueId id) : base(id)
    {
    }

    public Issue(
        IssueId id,
        Title title,
        Description description,
        LessonId lessonId,
        Experience experience,
        IEnumerable<FileInfo>? files = null) : base(id)
    {
        Title = title;
        Description = description;
        LessonId = lessonId;
        Experience = experience;
        _filesInfo = files?.ToList() ?? [];
    }

    public Experience Experience { get; private set; } = default!;
    public Title Title { get; private set; } = default!;
    public Description Description { get; private set; } = default!;

    public Position Position { get; private set; } = null!;

    public LessonId LessonId { get; private set; }

    public IssueType Type { get; private set; } = default!;

    public DateTime CreatedAt { get; private set; }

    public IReadOnlyList<FileInfo> FilesInfo { get; private set; } = null!;

    public void UploadFiles(IEnumerable<FileInfo> files)
    {
        _filesInfo = files.ToList();
    }

    public void SetPosition(Position position) =>
        Position = position;

    public UnitResult<Error> MoveForward()
    {
        var newPosition = Position.Forward();
        if (newPosition.IsFailure)
            return newPosition.Error;

        Position = newPosition.Value;

        return Result.Success<Error>();
    }

    public UnitResult<Error> MoveBack()
    {
        var newPosition = Position.Back();
        if (newPosition.IsFailure)
            return newPosition.Error;

        Position = newPosition.Value;

        return Result.Success<Error>();
    }

    public void Move(Position newPosition) =>
        Position = newPosition;

    internal UnitResult<Error> UpdateMainInfo(
        Title title,
        Description description,
        LessonId lessonId,
        Experience experience)
    {
        Title = title;
        Description = description;
        LessonId = lessonId;
        Experience = experience;

        return Result.Success<Error>();
    }
}

public enum IssueType
{
    Optional,
    Mandatory
}