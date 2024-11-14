using SachkovTech.Core.Abstractions;
using SachkovTech.Files.Contracts.Dtos;

namespace SachkovTech.Issues.Application.Features.Module.Commands.UploadFilesToIssue;

public record UploadFilesToIssueCommand(Guid ModuleId, Guid IssueId, IEnumerable<UploadFileDto> Files) : ICommand;