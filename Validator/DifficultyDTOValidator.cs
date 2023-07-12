using FluentValidation;

namespace UdemyProject.Validator
{
    public class DifficultyDTOValidator : AbstractValidator<Models.DTO.DifficultyDTO>
    {
        public DifficultyDTOValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
