// using CSharpFunctionalExtensions;
// using SachkovTech.Core.Abstractions;
// using SachkovTech.SharedKernel;
//
// namespace SachkovTech.Issues.Application.Features.Lessons.Command.UploadVideo;
//
// public record UploadVideoCommand(string FileName, long Size, string ContentType) : ICommand;
//
// public class UploadVideoHandler : ICommandHandler<Guid, UploadVideoCommand>
// {
//
//     public Task<Result<Guid, ErrorList>> Handle(UploadVideoCommand command, CancellationToken cancellationToken = default)
//     {
//
//     }
// }