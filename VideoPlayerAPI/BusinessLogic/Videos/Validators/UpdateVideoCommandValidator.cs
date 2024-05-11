using FluentValidation;
using VideoPlayerAPI.Abstractions.Repositories;
using VideoPlayerAPI.BusinessLogic.Videos.Commands;

namespace VideoPlayerAPI.BusinessLogic.Videos.Validators;

public class UpdateVideoCommandValidator : AbstractValidator<UpdateVideoCommand>
{
    private readonly IVideoRepository _videoRepository;

    public UpdateVideoCommandValidator(IVideoRepository videoRepository)
    {
        _videoRepository = videoRepository;

        RuleFor(command => command)
                .Must(ValidateVideo)
                .WithMessage("Video does not exist");
        RuleFor(command => command.Title)
            .NotEmpty()
            .WithMessage("Title cannot be empty");
        RuleFor(command => command.Description)
            .NotEmpty()
            .WithMessage("Description cannot be empty");
    }

    private bool ValidateVideo(UpdateVideoCommand command)
    {
        var video = _videoRepository.GetVideoByIdAsync(command.Id);

        if (video == null)
        {
            return false;
        }

        return true;
    }
}
