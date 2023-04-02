using FluentValidation;

namespace UdemyProject.Validator
{
    public class AddDifficultyRequestValidator: AbstractValidator<Models.DTO.AddDifficultyRequest>
    {
        public AddDifficultyRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }

    }
}
