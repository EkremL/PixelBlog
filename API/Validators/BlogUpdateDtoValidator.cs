using API.DTOs;
using FluentValidation;

namespace API.Validators
{

    public class BlogUpdateDtoValidator : AbstractValidator<BlogUpdateDto>
    {

        public BlogUpdateDtoValidator()
        {
            RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MinimumLength(20).WithMessage("Title must be at least 20 characters.")
            .MaximumLength(120).WithMessage("Title must not exceed 120 characters.");

            RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MinimumLength(200).WithMessage("Description must be at least 200 characters long.");

            RuleFor(x => x.Subtitle)
            .MaximumLength(150)
            .WithMessage("Subtitle must not exceed 150 characters.");

            RuleFor(x => x.ImageUrls)
            .Must(list => list == null || list.Count <= 10)
            .WithMessage("You can upload a maximum of 10 images.");
        }
    }
}