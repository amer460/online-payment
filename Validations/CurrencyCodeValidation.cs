using Application.HelperModels;
using System.ComponentModel.DataAnnotations;

namespace XCoreAssignment.Validations;

public class CurrencyCodeValidation
{
    public static ValidationResult ValidateCurrencyCode(string currencyCode, ValidationContext context)
    {
        var currency = CurrencyHelper.LookupByCode(currencyCode);

        if (currency.Code != "NotFound")
        {
            return ValidationResult.Success;
        }
        else
        {
            return new ValidationResult($"{currencyCode} is invalid currency code (try EUR, USD, JPY...)");
        }
    }
}
