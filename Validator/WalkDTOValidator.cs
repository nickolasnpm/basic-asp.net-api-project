using FluentValidation;

namespace UdemyProject.Validator
{
    public class WalkDTOValidator: AbstractValidator<Models.DTO.WalkDTO>
    {
        public WalkDTOValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Length).NotEmpty();
        }
    }
}
