using FluentValidation;

namespace MovieCore.ModelValidation.Movie.Validation;

public class MovieCreationValidation : AbstractValidator<MovieCreation>
{
    public MovieCreationValidation()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(50)
            .WithMessage("Title must not exceed 50 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters");

        RuleFor(x => x.ReleaseDate.Year)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.UtcNow.Year)
            .WithMessage("Release date is required");

        RuleFor(x => x.StreamTime)
            .NotEmpty()
            .WithMessage("Stream time is required");
    }
}