using System.ComponentModel.DataAnnotations;

namespace CC_Karriarpartner.Services.ValidationServices
{
    public interface IValidationService
    {
        (bool IsValid, List<string> ErrorMessages) ValidateModel<T>(T model);
    }

    public class ValidationService : IValidationService
    {
        public (bool IsValid, List<string> ErrorMessages) ValidateModel<T>(T model)
        {
            var validationContext = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            var errorMessages = new List<string>();
            if (!isValid)
            {
                foreach (var validationResult in validationResults)
                {
                    errorMessages.Add(validationResult.ErrorMessage);
                }
            }

            return (isValid, errorMessages);
        }
    }
}
