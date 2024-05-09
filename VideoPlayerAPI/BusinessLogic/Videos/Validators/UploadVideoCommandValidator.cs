using FluentValidation;
using VideoPlayerAPI.BusinessLogic.Videos.Queries;

namespace VideoPlayerAPI.BusinessLogic.Videos.Validators;

public class UploadVideoCommandValidator : AbstractValidator<UploadVideoCommand>
{
    public UploadVideoCommandValidator()
    {
        RuleFor(command => command.Title)
            .NotEmpty()
            .WithMessage("Title cannot be empty");
        RuleFor(command => command.Description)
            .NotEmpty()
            .WithMessage("Description cannot be empty");
        RuleFor(command => command)
            .Must(ValidateVideoData)
            .WithMessage("Video cannot be null");
    }

    private bool ValidateVideoData(UploadVideoCommand command)
    {
        if (command.VideoData == null || command.VideoData.Bytes.Length == 0)
        {
            return false;
        }

        return true;
    }
}
