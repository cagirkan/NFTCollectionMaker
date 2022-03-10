using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.ValidationRules
{
    public class CollectionValidator : AbstractValidator<Collection>
    {
        public CollectionValidator()
        {
            RuleFor(x => x.CollectionName)
                .NotEmpty()
                .WithMessage("Collection name must not empty!")
                .MaximumLength(50)
                .WithMessage("Collection name must be 50 characters max.!");
        }
    }
}
