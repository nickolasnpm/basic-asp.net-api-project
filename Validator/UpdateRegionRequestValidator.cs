using FluentValidation;

namespace UdemyProject.Validator
{
    public class UpdateRegionRequestValidator: AbstractValidator<Models.DTO.UpdateRegionRequest>
    {
        public UpdateRegionRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            // RuleFor(x => x.Area).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Area).GreaterThan(0);
            // RuleFor(x => x.Population).NotEmpty().GreaterThanOrEqualTo(0);
            RuleFor(x => x.Population).GreaterThanOrEqualTo(0);
        }
    }
}
