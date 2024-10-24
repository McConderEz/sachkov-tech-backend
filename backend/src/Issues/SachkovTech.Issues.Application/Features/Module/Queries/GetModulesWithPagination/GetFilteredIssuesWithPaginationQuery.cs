using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Features.Module.Queries.GetModulesWithPagination;

public record GetFilteredIssuesWithPaginationQuery(
    string? Title,
    int? PositionFrom,
    int? PositionTo,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize) : IQuery;