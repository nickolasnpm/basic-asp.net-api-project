using FluentValidation;

namespace UdemyProject.Validator
{
    public class UpdateDifficultyRequestValidator: AbstractValidator<Models.DTO.AddDifficultyRequest>
    {
        public UpdateDifficultyRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
