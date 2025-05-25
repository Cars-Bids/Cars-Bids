using FluentValidation;
using WebApiCarsBids.Models.Category;

namespace WebApiCarsBids.Validators.Category;

public class CategoryCreateValidator : AbstractValidator<CategoryCreateModel>
{
    public CategoryCreateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Назва є обов'язковою")
            .Must(name => !string.IsNullOrEmpty(name))
            .WithMessage("Назва не може бути порожньою або null")            
            .MaximumLength(250)
            .WithMessage("Назва повинна містити не більше 250 символів");
    }
}
