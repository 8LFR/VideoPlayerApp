using FluentValidation;
using VideoPlayerAPI.Abstractions.Repositories;
using VideoPlayerAPI.BusinessLogic.Account.Commands;

namespace VideoPlayerAPI.BusinessLogic.Account.Validators;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    private readonly IUserRepository _userRepository;

    public RegisterUserCommandValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("Username cannot be empty");
        RuleFor(command => command.Password)
            .NotEmpty()
            .WithMessage("Password cannot be empty");
        RuleFor(command => command)
            .Must(ValidateUser)
            .WithMessage("Username is already taken");
    }

    private bool ValidateUser(RegisterUserCommand command)
    {
        var user = _userRepository.GetUserByNameAsync(command.Name);

        if (user == null)
        {
            return true;
        }

        return false;
    }
}
