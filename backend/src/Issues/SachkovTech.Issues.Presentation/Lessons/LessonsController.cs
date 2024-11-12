using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using SachkovTech.Core.Dtos;
using SachkovTech.Framework;
using SachkovTech.Framework.Authorization;
using SachkovTech.Issues.Application.Features.Lessons.Command.AddIssueToLesson;
using SachkovTech.Issues.Application.Features.Lessons.Command.AddLesson;
using SachkovTech.Issues.Application.Features.Lessons.Command.AddTagToLesson;
using SachkovTech.Issues.Application.Features.Lessons.Command.RemoveIssueFromLesson;
using SachkovTech.Issues.Application.Features.Lessons.Command.RestoreLesson;
using SachkovTech.Issues.Application.Features.Lessons.Command.SoftDeleteLesson;
using SachkovTech.Issues.Application.Features.Lessons.Command.UpdateLesson;
using SachkovTech.Issues.Application.Features.Lessons.Queries.GetLessonById;
using SachkovTech.Issues.Application.Features.Lessons.Queries.GetLessonWithPagination;
using SachkovTech.Issues.Presentation.Lessons.Requests;

namespace SachkovTech.Issues.Presentation.Lessons;

public class LessonsController : ApplicationController
{
    [HttpPost]
    [Permission(Permissions.Lessons.CreateLesson)]
    public async Task<IActionResult> CreateLesson(
        [FromBody] AddLessonRequest request,
        [FromServices] AddLessonHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut]
    [Permission(Permissions.Lessons.UpdateLesson)]
    public async Task<IActionResult> UpdateLesson(
        [FromBody] UpdateLessonRequest request,
        [FromServices] UpdateLessonHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpPatch("{lessonId}/tag")]
    [Permission(Permissions.Lessons.UpdateLesson)]
    public async Task<IActionResult> AddTagToLesson(
        [FromRoute] Guid lessonId,
        [FromBody] Guid tagId,
        [FromServices] AddTagToLessonHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new AddTagToLessonCommand(lessonId, tagId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpPatch("{lessonId}/issue")]
    [Permission(Permissions.Lessons.UpdateLesson)]
    public async Task<IActionResult> AddIssueToLesson(
        [FromRoute] Guid lessonId,
        [FromBody] Guid issueId,
        [FromServices] AddIssueToLessonHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new AddIssueToLessonCommand(lessonId, issueId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpDelete("{lessonId}/issue")]
    [Permission(Permissions.Lessons.UpdateLesson)]
    public async Task<IActionResult> RemoveIssueFromLesson(
        [FromRoute] Guid lessonId,
        [FromBody] Guid issueId,
        [FromServices] RemoveIssueFromLessonHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new RemoveIssueFromLessonCommand(lessonId, issueId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpDelete("{lessonId}/tag")]
    [Permission(Permissions.Lessons.UpdateLesson)]
    public async Task<IActionResult> RemoveTagFromLesson(
        [FromRoute] Guid lessonId,
        [FromBody] Guid tagId,
        [FromServices] RemoveIssueFromLessonHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new RemoveIssueFromLessonCommand(lessonId, tagId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpPatch("{lessonId}/restore")]
    [Permission(Permissions.Lessons.UpdateLesson)]
    public async Task<IActionResult> RestoreLesson(
        [FromRoute] Guid lessonId,
        [FromServices] RestoreLessonHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new RestoreLessonCommand(lessonId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpDelete("{lessonId:guid}")]
    [Permission(Permissions.Lessons.DeleteLesson)]
    public async Task<IActionResult> SoftDeleteLesson(
        [FromRoute] Guid lessonId,
        [FromServices] SoftDeleteLessonHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new SoftDeleteLessonCommand(lessonId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpGet]
    //[Permission(Permissions.Lessons.ReadLesson)]
    public async Task<List<LessonDto>> GetLessonWithPagination(
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromServices] GetLessonsWithPaginationHandler handler,
        CancellationToken cancellationToken)
    {
        // var result = await handler.Handle(new GetLessonsWithPaginationValidatorQuery(page, pageSize),
        //     cancellationToken);
        //
        // if (result.IsFailure)
        //     return result.Error.ToResponse();

        // return Ok(result.Value);

        List<LessonDto> result =
        [
            new()
            {
                Id = Guid.NewGuid(),
                Description = "Description 1",
                Title = "Title 1",
                Experience = 1,
                VideoUrl = "VideoUrl 1",
                PreviewUrl = "PreviewUrl 1",
                Tags = [],
                Issues = []
            },
            new()
            {
                Id = Guid.NewGuid(),
                Description = "Description 2",
                Title = "Title 2",
                Experience = 2,
                VideoUrl = "VideoUrl 2",
                PreviewUrl = "PreviewUrl 2",
                Tags = [],
                Issues = []
            },
            new()
            {
                Id = Guid.NewGuid(),
                Description = "Description 3",
                Title = "Title 3",
                Experience = 3,
                VideoUrl = "VideoUrl 3",
                PreviewUrl = "PreviewUrl 3",
                Tags = [],
                Issues = []
            },
            new()
            {
                Id = Guid.NewGuid(),
                Description = "Description 4",
                Title = "Title 4",
                Experience = 4,
                VideoUrl = "VideoUrl 4",
                PreviewUrl = "PreviewUrl 4",
                Tags = [],
                Issues = []
            },
            new()
            {
                Id = Guid.NewGuid(),
                Description = "Description 5",
                Title = "Title 5",
                Experience = 5,
                VideoUrl = "VideoUrl 5",
                PreviewUrl = "PreviewUrl 5",
                Tags = [],
                Issues = []
            },
            new()
            {
                Id = Guid.NewGuid(),
                Description = "Description 6",
                Title = "Title 6",
                Experience = 6,
                VideoUrl = "VideoUrl 6",
                PreviewUrl = "PreviewUrl 6",
                Tags = [],
                Issues = []
            },
            new()
            {
                Id = Guid.NewGuid(),
                Description = "Description 7",
                Title = "Title 7",
                Experience = 7,
                VideoUrl = "VideoUrl 7",
                PreviewUrl = "PreviewUrl 7",
                Tags = [],
                Issues = []
            },
            new()
            {
                Id = Guid.NewGuid(),
                Description = "Description 8",
                Title = "Title 8",
                Experience = 8,
                VideoUrl = "VideoUrl 8",
                PreviewUrl = "PreviewUrl 8",
                Tags = [],
                Issues = []
            },
            new()
            {
                Id = Guid.NewGuid(),
                Description = "Description 9",
                Title = "Title 9",
                Experience = 9,
                VideoUrl = "VideoUrl 9",
                PreviewUrl = "PreviewUrl 9",
                Tags = [],
                Issues = []
            },
            new()
            {
                Id = Guid.NewGuid(),
                Description = "Description 10",
                Title = "Title 10",
                Experience = 10,
                VideoUrl = "VideoUrl 10",
                PreviewUrl = "PreviewUrl 10",
                Tags = [],
                Issues = []
            },
        ];


        return result;
    }

    [HttpGet("{lessonId:guid}")]
    [Permission(Permissions.Lessons.ReadLesson)]
    public async Task<IActionResult> GetLessonById(
        [FromRoute] Guid lessonId,
        [FromServices] GetLessonByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetLessonByIdQuery(lessonId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}