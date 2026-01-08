using System.ComponentModel.DataAnnotations;

namespace CampusEats.Frontend.Shared.Validation;

public class CapitalizedFirstLetterAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null) return ValidationResult.Success;

        var s = value as string;
        if (string.IsNullOrWhiteSpace(s)) return ValidationResult.Success;

        if (char.IsUpper(s[0])) return ValidationResult.Success;
        
        var message = string.IsNullOrEmpty(ErrorMessage) ? "The first letter must be uppercase." : ErrorMessage;
        return new ValidationResult(message);
    }
}
