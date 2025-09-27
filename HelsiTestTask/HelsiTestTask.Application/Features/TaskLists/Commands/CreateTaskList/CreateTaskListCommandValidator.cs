using FluentValidation;

namespace HelsiTestTask.Application.Features.TaskLists.Commands.CreateTaskList;

public class CreateTaskListCommandValidator: AbstractValidator<CreateTaskListCommand>
{
    public CreateTaskListCommandValidator()
    {
        RuleFor(x => x.Body)
            .NotNull();
        
        RuleFor(x => x.Body)
            .SetValidator(new CreateTaskListCommandBodyValidator());
    }
}

public class CreateTaskListCommandBodyValidator : AbstractValidator<CreateTaskListBody>
{
    private const int NameMaxLength = 250;
    public CreateTaskListCommandBodyValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(NameMaxLength);
    }
}
