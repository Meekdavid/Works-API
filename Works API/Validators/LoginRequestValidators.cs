using FluentValidation;

namespace Works_API.Validators
{
    public class LoginRequestValidators : AbstractValidator<Models.DTO.LoginRequest>
    {
        public LoginRequestValidators()
        {
            RuleFor(x=>x.Username).NotEmpty();
            RuleFor(x=>x.password).NotEmpty();
        }
    }
}
