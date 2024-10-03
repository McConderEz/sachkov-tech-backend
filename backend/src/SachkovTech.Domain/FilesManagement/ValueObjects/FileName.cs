using CSharpFunctionalExtensions;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Domain.FilesManagement.ValueObjects;

public class FileName : ValueObject
{
    public string Value { get; }

    private FileName(string value)
    {
        Value = value;
    }

    public static Result<FileName, Error> Create(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return Errors.General.ValueIsInvalid("file name");

        return new FileName(fileName);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(FileName fileName) =>
        fileName.Value;
}