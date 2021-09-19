using Core.DTO;
using FluentValidation;

namespace Core.Validations
{
    public class UserValidation:AbstractValidator<UserDto>
    {
        public UserValidation()
        {
            RuleFor(x => x.FirstName)
                .MaximumLength(30)
                .NotEmpty();
            RuleFor(x => x.LastName)
                .MaximumLength(30)
                .NotEmpty();
            RuleFor(x => x.Email)
                .MaximumLength(30)
                .EmailAddress()
                .NotEmpty();
        }
    }
}