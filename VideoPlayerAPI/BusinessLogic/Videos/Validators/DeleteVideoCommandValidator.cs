using FluentValidation;
using VideoPlayerAPI.Abstractions.Repositories;
using VideoPlayerAPI.BusinessLogic.Videos.Commands;

namespace VideoPlayerAPI.BusinessLogic.Videos.Validators
{
    public class DeleteVideoCommandValidator : AbstractValidator<DeleteVideoCommand>
    {
        private readonly IVideoRepository _videoRepository;

        public DeleteVideoCommandValidator(IVideoRepository videoRepository)
        {
            _videoRepository = videoRepository;

            RuleFor(command => command)
                .Must(ValidateVideo)
                .WithMessage("Video does not exist");
        }

        private bool ValidateVideo(DeleteVideoCommand command)
        {
            var video = _videoRepository.GetVideoByIdAsync(command.Id);

            if (video == null)
            {
                return false;
            }

            return true;
        }
    }
}
