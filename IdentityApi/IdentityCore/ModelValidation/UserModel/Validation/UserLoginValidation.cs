using FluentValidation;

namespace IdentityCore.ModelValidation.UserModel.Validation;

public class UserLoginValidation : AbstractValidator<UserLogin>
{
    public UserLoginValidation()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required")
            .MaximumLength(50)
            .WithMessage("Username must not exceed 50 characters");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters")
            .MaximumLength(50)
            .WithMessage("Password must not exceed 50 characters");

        RuleFor(x => x)
            .Must(x => string.IsNullOrEmpty(x.Username) != string.IsNullOrEmpty(x.Email))
            .WithMessage("Either Username or Email must be provided, but not both.");
    }
}