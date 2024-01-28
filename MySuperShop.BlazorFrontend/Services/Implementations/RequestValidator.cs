using MySuperShop.BlazorFrontend.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MySuperShop.BlazorFrontend.Services.Implementations
{
    public class RequestValidator : IValidator
    {
        public void Validate<T>(object value)
        {
            var validationContext = new ValidationContext((T)value);
            var validationResults = new List<ValidationResult>();//возможно стоит возвращать результат (для юзер френдли)
            if (!Validator.TryValidateObject(value, validationContext, validationResults, true))
            {
                throw new ValidationException(nameof(value));
            }
        }
    }
}
