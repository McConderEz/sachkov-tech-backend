﻿using CSharpFunctionalExtensions;
using SachkovTech.SharedKernel;

namespace SachkovTech.Accounts.Domain.ValueObjects;

public class Photo : ValueObject
{
    private static string[] PERMITED_FILES_TYPE =
            { "image/jpg", "image/jpeg", "image/png", "image/gif" };

    private static string[] PERMITED_EXTENSIONS =
            { "jpg", "jpeg", "png", "gif" };

    private static long MAX_FILE_SIZE = 5242880;

    private Photo(Guid fileId)
    {
        FileId = fileId;      
    }

    public Guid FileId { get; }    

    public static Result<Photo, Error> Create(Guid fileId) => new Photo(fileId);    

    public static UnitResult<Error> Validate(
        string fileName,
        string contentType,
        long size)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return Errors.General.ValueIsInvalid(fileName);
        }

        var fileExtension = fileName[fileName.LastIndexOf('.')..];

        if (!PERMITED_EXTENSIONS.Any(x => x == fileExtension))
        {
            return Errors.Files.InvalidExtension();
        }

        if (!PERMITED_FILES_TYPE.Any(x => x == contentType))
        {
            return Errors.General.ValueIsInvalid(contentType);
        }

        if (size > MAX_FILE_SIZE)
        {
            return Errors.Files.InvalidSize();
        }

        return Result.Success<Error>();
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return FileId;
    }
}