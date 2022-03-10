using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.ValidationRules
{
    public class ArtworkValidator : AbstractValidator<Artwork>
    {
        public ArtworkValidator()
        {
            RuleFor(x => x.ArtworkName)
                .NotEmpty()
                .WithMessage("Artwork name must not empty!")
                .MaximumLength(50)
                .WithMessage("Artwork name must be 50 characters max.!");
            RuleFor(x => x.ImageURL)
                .NotEmpty()
                .WithMessage("Artwork image must not empty!");
        }
    }
}
