using FluentValidation;

namespace HelsiTestTask.Application.Features.TaskLists.Commands.UpdateTaskList;

public class UpdateTaskListCommandValidator: AbstractValidator<UpdateTaskListCommand>
{
    public UpdateTaskListCommandValidator()
    {
        RuleFor(x => x.Body)
            .NotNull();
        
        RuleFor(x => x.Body)
            .SetValidator(new UpdateTaskListBodyValidator());
    }
}

public class UpdateTaskListBodyValidator: AbstractValidator<UpdateTaskListBody>
{
    private const int NameMaxLength = 250;
    public UpdateTaskListBodyValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(NameMaxLength);
    }
}
