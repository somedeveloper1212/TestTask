using FluentValidation;

namespace HelsiTestTask.Application.Features.TaskLists.Queries.GetTaskLists;

public class GetTaskListsQueryValidator: AbstractValidator<GetTaskListsQuery>
{
    public GetTaskListsQueryValidator()
    {
        RuleFor(q => q.Skip)
            .GreaterThanOrEqualTo(0);

        RuleFor(q => q.Take)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);
    }
}
