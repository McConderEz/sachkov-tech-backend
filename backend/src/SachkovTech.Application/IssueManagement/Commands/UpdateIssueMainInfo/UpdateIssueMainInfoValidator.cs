﻿using FluentValidation;
using SachkovTech.Application.Validation;
using SachkovTech.Domain.Shared;
using SachkovTech.Domain.Shared.ValueObjects;

namespace SachkovTech.Application.IssueManagement.Commands.UpdateIssueMainInfo;

public class UpdateIssueMainInfoValidator : AbstractValidator<UpdateIssueMainInfoCommand>
{
    public UpdateIssueMainInfoValidator()
    {
        RuleFor(u => u.ModuleId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.IssueId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.Title).MustBeValueObject(Title.Create);
        RuleFor(u => u.Description).MustBeValueObject(Description.Create);
        RuleFor(u => u.Experience).GreaterThanOrEqualTo(1).LessThanOrEqualTo(1000);
    }
    
}