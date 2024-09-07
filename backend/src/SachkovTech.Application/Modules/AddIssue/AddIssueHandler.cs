using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SachkovTech.Application.Database;
using SachkovTech.Application.Extensions;
using SachkovTech.Application.FileProvider;
using SachkovTech.Domain.IssueManagement.Entities;
using SachkovTech.Domain.IssueManagement.ValueObjects;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Shared.ValueObjects;
using SachkovTech.Domain.Shared.ValueObjects.Ids;

namespace SachkovTech.Application.Modules.AddIssue;

public class AddIssueHandler
{
    private const string BUCKET_NAME = "files";

    private readonly IFileProvider _fileProvider;
    private readonly IModulesRepository _modulesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddIssueCommand> _validator;
    private readonly ILogger<AddIssueHandler> _logger;

    public AddIssueHandler(
        IFileProvider fileProvider,
        IModulesRepository modulesRepository,
        IUnitOfWork unitOfWork,
        IValidator<AddIssueCommand> validator,
        ILogger<AddIssueHandler> logger)
    {
        _fileProvider = fileProvider;
        _modulesRepository = modulesRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddIssueCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToList();
        }

        var moduleResult = await _modulesRepository
            .GetById(ModuleId.Create(command.ModuleId), cancellationToken);

        if (moduleResult.IsFailure)
            return moduleResult.Error.ToErrorList();

        var issue = InitIssue(command);
        moduleResult.Value.AddIssue(issue);

        await _unitOfWork.SaveChanges(cancellationToken);

        return issue.Id.Value;
    }

    private Issue InitIssue(AddIssueCommand command)
    {
        var issueId = IssueId.NewIssueId();
        var title = Title.Create(command.Title).Value;
        var description = Description.Create(command.Description).Value;
        var lessonId = LessonId.Empty();
        
        return new Issue(
            issueId,
            title,
            description,
            lessonId,
            null,
            null);
    }
}