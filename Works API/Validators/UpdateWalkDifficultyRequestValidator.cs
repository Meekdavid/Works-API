using FluentValidation;

namespace Works_API.Validators
{
    public class UpdateWalkDifficultyRequestValidator : AbstractValidator<Models.DTO.AddWalkDifficultyRequest>
    {
        public UpdateWalkDifficultyRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
