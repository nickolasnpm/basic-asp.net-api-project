using FluentValidation;

namespace UdemyProject.Validator
{
    public class RegionDTOValidator : AbstractValidator<Models.DTO.RegionDTO>
    {
        public RegionDTOValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Area).GreaterThan(0);
            RuleFor(x => x.Population).GreaterThanOrEqualTo(0);
        }
    }
}
