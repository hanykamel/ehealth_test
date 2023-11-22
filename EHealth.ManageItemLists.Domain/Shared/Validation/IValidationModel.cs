using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHealth.ManageItemLists.Domain.Shared.Validation
{
    public interface IValidationModel<T>
    {
        AbstractValidator<T> Validator { get; }
        bool IsValid => Validator.ValidateAsync((T)this).Result.IsValid;
        List<ValidationFailure>? ValidationErrors => Validator.ValidateAsync((T)this).Result.Errors;
    }
}
