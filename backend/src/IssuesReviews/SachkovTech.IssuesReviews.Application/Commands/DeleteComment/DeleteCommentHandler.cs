using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Extensions;
using SachkovTech.IssuesReviews.Application.Commands.AddComment;
using SachkovTech.SharedKernel;
using SachkovTech.SharedKernel.ValueObjects.Ids;

namespace SachkovTech.IssuesReviews.Application.Commands.DeleteComment;

public class DeleteCommentHandler : ICommandHandler<Guid, DeleteCommentCommand>
{
    private readonly IIssueReviewRepository _issueReviewRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeleteCommentCommand> _validator;
    private readonly ILogger<DeleteCommentHandler> _logger;

    public DeleteCommentHandler(
        IIssueReviewRepository issueReviewRepository,
        [FromKeyedServices(Modules.IssuesReviews)] IUnitOfWork unitOfWork,
        IValidator<DeleteCommentCommand> validator,
        ILogger<DeleteCommentHandler> logger)
    {
        _issueReviewRepository = issueReviewRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteCommentCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToList();
        }

        var issueReviewResult = await _issueReviewRepository
            .GetById(IssueReviewId.Create(command.IssueReviewId), cancellationToken);

        if (issueReviewResult.IsFailure)
            return issueReviewResult.Error.ToErrorList();

        var commentId = CommentId.Create(command.CommentId);

        var userId = UserId.Create(command.UserId);

        var addCommentResult = issueReviewResult.Value.DeleteComment(commentId, userId);

        if (addCommentResult.IsFailure)
            return addCommentResult.Error.ToErrorList();

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation(
            "Comment {commentId} was deleted in issueReview {issueReviewId}",
            command.CommentId,
            command.IssueReviewId);

        return command.CommentId;
    }
}