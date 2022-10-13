Efficient effi
﻿using FluentValidation;

namespace Works_API.Validators
{
    public class AddRegionRequestValidator : AbstractValidator<Models.DTO.AddRegionRequest>
    {
        public AddRegionRequestValidator()
        {
            RuleFor(x=>x.Code).NotEmpty();
            RuleFor(x=>x.Name).NotEmpty();
            RuleFor(x=>x.Area).GreaterThan(0);
            RuleFor(x=>x.Population).GreaterThanOrEqualTo(0);
        }
    }
}
