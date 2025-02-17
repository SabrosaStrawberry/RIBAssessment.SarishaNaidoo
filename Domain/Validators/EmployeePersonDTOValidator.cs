using Domain.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Validators
{
    public class EmployeePersonDTOValidator : AbstractValidator<EmployeePersonDTO>
    {
        public EmployeePersonDTOValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is Required")
                .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

            RuleFor(x => x.LastName)
               .NotEmpty().WithMessage("Last name is Required")
               .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");

            RuleFor(x => x.EmployeeNumber)
               .NotEmpty().WithMessage("Employee Number is Required")
               .MaximumLength(16).WithMessage("Employee Number cannot exceed 16 characters");

            RuleFor(x => x.BirthDate)
               .NotEmpty().WithMessage("Date of Birth is Required");

            RuleFor(x => x.EmployedDate)
               .NotEmpty().WithMessage("Employed Date is Required");
        }
    }
}
