using Frontend.Services.Interfaces;
using Models;
using System.ComponentModel.DataAnnotations;

namespace Frontend.Services.Implementations
{
    public class RegistrationRequestValidator : IValidator
    {
        public void Validate(object value)
        {
            var validationContext = new ValidationContext((RegistrationRequest)value);
            var validationResults = new List<ValidationResult>();//возможно стоит возвращать результат (для юзер френдли)
            if (!Validator.TryValidateObject(value, validationContext, validationResults, true))
            {
                throw new ValidationException(nameof(value));
            }
        }
    }
}
