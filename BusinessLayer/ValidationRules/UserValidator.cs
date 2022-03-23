using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.ValidationRules
{
    public class UserValidator : AbstractValidator<User>
    {
        UserManager um = new UserManager(new EfUserRepository());
        public UserValidator()
        {
            RuleFor(x => x.UserName)
               .NotEmpty()
               .WithMessage("Username must be filled!")
               .MinimumLength(3)
               .WithMessage("Username must contain at least 3 characters!")
               .Must(IsUnameUnique)
               .WithMessage("This username is already taken!");
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Please enter a valid e-mail address!")
                .Must(IsEmailUnique)
                .WithMessage("Already registered with this e-mail!");
        }

        private bool IsUnameUnique(string username)
        {
            return (um.isUsernameUnique(username));
        }

        private bool IsEmailUnique(string email)
        {
            return (um.isEmailUnique(email));
        }
    }
}
