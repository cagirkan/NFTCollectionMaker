using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.ValidationRules
{
    public class CollectionLayerValidator : AbstractValidator<CollectionLayer>
    {
        public CollectionLayerValidator()
        {
            RuleFor(x => x.CollectionLayerName)
                .NotEmpty()
                .WithMessage("Tag must be filled!");
        }
    }
}
