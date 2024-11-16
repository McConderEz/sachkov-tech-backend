﻿namespace SachkovTech.Issues.Contracts.Responses
{
    public class IssueResponse
    {
        public Guid Id { get; init; }

        public Guid ModuleId { get; init; }

        public string Title { get; init; } = default!;

        public string Description { get; init; } = default!;

        public int? Position { get; init; }

        public Guid LessonId { get; init; }
    }
}