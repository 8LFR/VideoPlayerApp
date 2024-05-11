using FluentValidation;
using System.Security.Cryptography;
using System.Text;
using VideoPlayerAPI.Abstractions.Repositories;
using VideoPlayerAPI.BusinessLogic.Account.Commands;

namespace VideoPlayerAPI.BusinessLogic.Account.Validators;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand> 
{
    private readonly IUserRepository _userRepository;

    public LoginUserCommandValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("Username cannot be empty");
        RuleFor(command => command.Password)
            .NotEmpty()
            .WithMessage("Password cannot be empty");
        RuleFor(command => command)
            .MustAsync(ValidateUser)
            .WithMessage("User does not exist");
        RuleFor(command => command)
            .MustAsync(ValidatePassword)
            .WithMessage("Invalid password");
    }

    private async Task<bool> ValidateUser(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByNameAsync(command.Name);

        if (user == null)
        {
            return false;
        }

        return true;
    }

    private async Task<bool> ValidatePassword(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByNameAsync(command.Name);

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(command.Password));

        for (var i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
            {
                return false;
            }
        }

        return true;
    }
}

