﻿using SachkovTech.Issues.Application.Features.Lessons.Command.UpdateLesson;

namespace SachkovTech.Issues.Presentation.Lessons.Requests;

public record UpdateLessonRequest(
    Guid LessonId,
    string Title,
    string Description,
    int Experience,
    Guid VideoId,
    Guid PreviewFileId,
    IEnumerable<Guid> Tags,
    IEnumerable<Guid> Issues)
{
    public UpdateLessonCommand ToCommand() => 
        new(LessonId, Title, Description, Experience, VideoId, PreviewFileId, Tags, Issues);
}
