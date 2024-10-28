using CSharpFunctionalExtensions;

namespace SachkovTech.SharedKernel.ValueObjects.Ids;

public class Image : ValueObject, IFileType
{
    public Image(float length, float width)
    {
        Length = length;
        Width = width;
    }

    public float Length { get; }
    public float Width { get; }

    public UnitResult<Error> CalculateSize()
    {
        return Length * Width > 100000 ? Errors.General.ValueIsInvalid("image") : UnitResult.Success<Error>();
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Length;
        yield return Width;
    }
}

public interface IFileType : IComparable
{
    UnitResult<Error> CalculateSize();
}

public class FileInfo : ValueObject
{
    public FileInfo(FileId id, IFileType type, string[] attributes)
    {
        Id = id;
        Type = type;
        Attributes = attributes;
    }

    public FileId Id { get; }

    public IFileType Type { get; }

    public string[] Attributes { get; } = ["image", "video"];

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Id;
        yield return Type;
    }
}

public class FileId : ValueObject
{
    private FileId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static FileId NewFileId() => new(Guid.NewGuid());
    public static FileId Empty() => new(Guid.Empty);
    public static FileId Create(Guid id) => new(id);

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}